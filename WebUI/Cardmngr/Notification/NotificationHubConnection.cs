using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Notification;

public sealed class NotificationHubConnection(IServiceProvider serviceProvider) : IAsyncDisposable
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private string lastUserId = string.Empty;   
    private HubConnection? connection;

    public bool Connected => connection is { State: HubConnectionState.Connected };

    public async Task StartAsync(string userId)
    {
        lastUserId = userId;
        connection = InitializeHubConnection();

        await connection.StartAsync();

        await connection.SendAsync("Join", userId);
    }

    public async Task ReconnectAsync()
    {
        await DisposeAsync();

        if (string.IsNullOrEmpty(lastUserId)) throw new InvalidOperationException("No user id to reconnect.");

        await StartAsync(lastUserId);
    }

    private HubConnection InitializeHubConnection()
    {
        if (connection is { }) throw new InvalidOperationException("Hub connection already initialized.");

        using var scope = serviceProvider.CreateScope();

        var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();

        var result = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri(HubPatterns.Notification))
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers(result);

        return result;
    }

    private void RegisterHandlers(HubConnection connection)
    {
        connection.On<OnlyofficeTask>(nameof(ReceiveTaskAsync), ReceiveTaskAsync);
    }

    public event Action<OnlyofficeTask>? TaskReceived;

    private void ReceiveTaskAsync(OnlyofficeTask task) => TaskReceived?.Invoke(task);

    public async Task NotifyAboutCreatedTaskAsync(OnlyofficeTask task)
    {
        if (connection is { })
        {
            if (connection.State == HubConnectionState.Disconnected)
            {
                await ReconnectAsync();
            }

            await connection.SendAsync(nameof(NotifyAboutCreatedTaskAsync), task, lastUserId);
        }
    }

    public ValueTask DisposeAsync()
    {
        if (connection is { })
        {
            return connection.DisposeAsync();
        }

        return default;
    }
}
