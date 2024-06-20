using Cardmngr.Server.Hubs;

namespace Cardmngr.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCardmngrServices(this IServiceCollection collection)
    {
        return collection.AddSingleton<GroupManager>();
    }
}
