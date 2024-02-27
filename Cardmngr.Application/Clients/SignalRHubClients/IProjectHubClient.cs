using Cardmngr.Shared.Hubs;

namespace Cardmngr.Application.Clients.SignalRHubClients;

public interface IProjectHubClient : IHubClient, IProjectHubSender, IProjectHubReceiver
{

}
