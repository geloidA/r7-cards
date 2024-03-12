using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Cardmngr.Server.Hubs;

public class NotificationHub : Hub<INotificationReceiver>
{
    private readonly Serilog.ILogger logger = Log.ForContext<NotificationHub>();

    public Task Join(string userId) // TODO: think about identity
    {
        logger.Information("Join. Context: {context}, UserId: {userId}", Context.ConnectionId, userId);

        return Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }

    public Task NotifyAboutCreatedTaskAsync(OnlyofficeTask task, string callerUserId)
    {
        logger.Information("NotifyAboutCreatedTaskAsync");

        foreach (var user in task.Responsibles.Where(u => callerUserId != u.Id))
        {
            logger.Information("NotifyAboutCreatedTaskAsync. From Context: {context} To UserId: {userId}", Context.ConnectionId, user.Id);
            _ = Clients.Group(user.Id).ReceiveTaskAsync(task);            
        }

        return Task.CompletedTask;
    }
}
