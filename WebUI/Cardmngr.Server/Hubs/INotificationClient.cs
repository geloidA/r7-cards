using Cardmngr.Domain.Entities;

namespace Cardmngr.Server;

public interface INotificationReceiver
{
    Task ReceiveTaskAsync(OnlyofficeTask task);
}
