using Cardmngr.Server.Hubs;
using Cardmngr.Shared.Hubs;

namespace Cardmngr.Server.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<ProjectBoardHub>(HubPatterns.ProjectBoard);
        app.MapHub<NotificationHub>(HubPatterns.Notification);

        return app;
    }

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
