using Cardmngr.Server.AppInfoApi.Service;
using Cardmngr.Server.FeedbackApi.Service;
using Cardmngr.Server.Hubs;

namespace Cardmngr.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCardmngrServices(this IServiceCollection collection)
    {
        return collection
            .AddSingleton<GroupManager>()
            .AddSingleton<NotificationManager>()
            .AddScoped<IFeedbackService, FeedbackService>()
            .AddScoped<IAppInfoService, AppInfoService>();
    }
}
