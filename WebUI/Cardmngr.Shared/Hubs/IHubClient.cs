namespace Cardmngr.Shared.Hubs;

public interface IHubClient : IAsyncDisposable
{
    Task StartAsync();
}
