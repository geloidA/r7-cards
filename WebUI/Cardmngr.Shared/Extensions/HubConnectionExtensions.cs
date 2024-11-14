using Microsoft.AspNetCore.SignalR.Client;

namespace Cardmngr.Shared.Extensions;

public static class HubConnectionExtensions
{
    public static async Task SendWithReconnectAsync(this HubConnection connection, string methodName, object? arg)
    {
        ArgumentNullException.ThrowIfNull(connection, nameof(connection));

        if (connection.State != HubConnectionState.Connected)
        {
            await connection.StartAsync();
        }

        await connection.SendAsync(methodName, arg);
    }
}
