using Cardmngr.Server.Hubs;
using Cardmngr.Shared.Hubs;

namespace Cardmngr.Server.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Маппинг эндпоинтов для хабов SignalR.
    /// </summary>
    /// <param name="app"></param>
    /// <remarks>
    ///     Маппинг эндпоинтов:
    ///     /board/{projectid} - проектный хаб.
    ///     /notification/{userid} - хаб уведомлений.
    /// </remarks>
    public static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<ProjectBoardHub>(HubPatterns.ProjectBoard);
        app.MapHub<NotificationHub>(HubPatterns.Notification);

        return app;
    }

    /// <summary>
    /// Маппинг эндпоинтов для собственного приложения
    /// </summary>
    /// <param name="app"></param>
    /// <remarks>
    ///     Маппинг эндпоинтов:
    ///     /appinfo/version/{current} - получение версии приложения.
    ///     Если текущая версия не равна полученной, то обновляется кеш
    /// </remarks>
    public static WebApplication MapSelfEndpoints(this WebApplication app)
    {
        app.MapGet("/appinfo/version/{current}", async (string current, HttpContext context) =>
        {
            var version = await File.ReadAllTextAsync("appversion");

            if (current != version)
            {
                context.Response.Headers.Append("Clear-Site-Data", "\"cache\"");
            }

            return version;
        });

        return app;
    }
}
