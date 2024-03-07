using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Cardmngr.Shared.Extensions;

namespace Cardmngr.Services;

public sealed class MyNotificationService(IServiceProvider serviceProvider) : IAsyncDisposable
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private HubConnection? connection;

    // public async Task TryRequestPermissionAsync(string userId)
    // {        
    //     using var scope = serviceProvider.CreateScope();
    //     var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

    //     var isSupportedByBrowser = await notificationService.IsSupportedByBrowserAsync();

    //     if (isSupportedByBrowser)
    //     {
    //         actualPermissionType = await notificationService.RequestPermissionAsync();

    //         if (actualPermissionType == PermissionType.Granted)
    //         {
    //             await StartConnectionAsync(userId);
    //         }
    //     }
    // }

    public bool HasConnection => connection is { State: HubConnectionState.Connected };

    private async Task StartConnectionAsync(string userId)
    {
        connection = InitializeHubConnection();

        await connection.StartAsync();

        await connection.SendAsync("Join", userId);
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


    private void ReceiveTaskAsync(OnlyofficeTask task)
    {
        Console.WriteLine("Received task !!!!!!!!!!!!!!!!!");

        TaskReceived?.Invoke(task);
    }

    public Task SendTaskAsync(OnlyofficeTask task)
    {
        return connection?.SendWithReconnectAsync(nameof(SendTaskAsync), task) ?? Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        if (connection is { })
        {
            Console.WriteLine("Dispose notification service");
            return connection.DisposeAsync();
        }
        else
        {
            return default;
        }
    }
}
