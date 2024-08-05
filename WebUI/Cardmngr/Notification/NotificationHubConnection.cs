using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Notification;

public sealed class NotificationHubConnection(IServiceProvider serviceProvider) : IAsyncDisposable
{
    private string lastUserId = string.Empty;
    private HubConnection? _connection;

    public bool Connected => _connection is { State: HubConnectionState.Connected };

    public async Task StartAsync(string userId)
    {
        lastUserId = userId;
        _connection = InitializeHubConnection();

        await _connection.StartAsync();

        await _connection.SendAsync("Join", userId);
    }

    private async Task ReconnectAsync()
    {
        await DisposeAsync();

        if (string.IsNullOrEmpty(lastUserId)) throw new InvalidOperationException("No user id to reconnect.");

        await StartAsync(lastUserId);
    }

    private HubConnection InitializeHubConnection()
    {
        if (_connection is not null) throw new InvalidOperationException("Hub connection already initialized.");

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
        if (!Connected)
        {
            await ReconnectAsync();
        }

        if (_connection is not null)
        {
            await _connection.SendAsync(nameof(NotifyAboutCreatedTaskAsync), task, lastUserId);
        }
    }

    public ValueTask DisposeAsync()
    {
        return _connection?.DisposeAsync() ?? default;
    }
}
