using BlazorBootstrap;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Utils;

public class ProjectStatusData(BadgeColor badgeColor, IconName iconName, string title)
{
    public BadgeColor BadgeColor => badgeColor;
    public IconName IconName => iconName;
    public string Title => title;

    public static ProjectStatusData Open => new(BadgeColor.Primary, IconName.ForwardFill, "Открыт");
    public static ProjectStatusData Closed => new(BadgeColor.Success, IconName.CheckCircleFill, "Закрыт");
    public static ProjectStatusData Paused => new(BadgeColor.Secondary, IconName.PauseCircleFill, "Приостановлен");
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
