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
}
