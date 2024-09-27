using Cardmngr.Domain.Entities;

namespace Cardmngr.Server.Hubs;

public interface INotificationReceiver
{
    Task ReceiveTaskAsync(OnlyofficeTask task);
}
