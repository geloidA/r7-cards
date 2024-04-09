using Cardmngr.Domain.Enums;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Utils;

public class ProjectStatusData(string badgeColor, Icon iconName, string title)
{
    public string BadgeColor => badgeColor;
    public Icon IconName => iconName;
    public string Title => title;

    public static ProjectStatusData Open => new("#A79277", new Icons.Filled.Size20.ArrowForward(), "Открыт");
    public static ProjectStatusData Closed => new("#5BBCFF", new Icons.Filled.Size20.CheckmarkCircle(), "Закрыт");
    public static ProjectStatusData Paused => new("#FFD1E3", new Icons.Filled.Size20.Pause(), "Приостановлен");
}

public static class ProjectStatusExtensions
{
    public static ProjectStatusData GetBadgeData(this ProjectStatus status)
    {
        return status switch
        {
            ProjectStatus.Paused => ProjectStatusData.Paused,
            ProjectStatus.Open => ProjectStatusData.Open,
            ProjectStatus.Closed => ProjectStatusData.Closed,
            _ => throw new NotImplementedException("Unknown ProjectStatus type")
        };
    }
}
