using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Cardmngr.Server.Hubs;

public class NotificationHub(NotificationManager clients) : Hub<INotificationReceiver>
{
    private readonly NotificationManager clients = clients;
    private readonly Serilog.ILogger logger = Log.ForContext<NotificationHub>();

    public void Join(string userId) // TODO: think about identity
    {
        logger.Information("Join. Context: {context}, UserId: {userId}", Context.ConnectionId, userId);
        clients.Add(Context.ConnectionId, userId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (clients.Remove(Context.ConnectionId))
        {
            logger.Information("Disconnected. Context: {context}, UserId: {userId}", Context.ConnectionId);
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    public Task SendTaskAsync(OnlyofficeTask task)
    {
        logger.Information(string.Format("SendTaskAsync. Responsibles {0}; ConnectedUsers: {1}", 
            string.Join(",", task.Responsibles.Single().Id),
            clients.ValuePairs.Select(x => x.Key).Aggregate((x, y) => $"{x}, {y}")));
        foreach (var user in task.Responsibles.Where(user => clients.ContainsValue(user.Id)))
        {
            if (clients.TryGetKeyByValue(user.Id, out var connectionId))
            {
                logger.Information("SendTaskAsync. From Context: {context} To UserId: {userId}", Context.ConnectionId, user.Id);
                _ = Clients.Others.ReceiveTaskAsync(task);
            }
        }

        return Task.CompletedTask;
    }
}
