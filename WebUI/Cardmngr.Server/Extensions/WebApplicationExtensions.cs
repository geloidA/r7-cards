using Cardmngr.Server.Hubs;

namespace Cardmngr.Server.Extensions;

public static class WebApplicationExtensions
{
    public static void MapHubs(this WebApplication app)
    {
        app.MapHub<ProjectBoardHub>("/hubs/projectboard");
    }
}
