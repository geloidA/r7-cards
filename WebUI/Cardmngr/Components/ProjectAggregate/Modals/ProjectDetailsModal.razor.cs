using BlazorBootstrap;
using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.MilestoneAggregate.Modals;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Modals;

public partial class ProjectDetailsModal
{
    private Components.Modals.MyBlazored.Offcanvas currentModal = null!;
    [Parameter] public ProjectState State { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")] ModalOptions Options { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    private void ShowMilestoneCreation()
    {
        var parameters = new ModalParameters
        {
            { "IsAdd", true },
            { "State", State }
        };

        Modal.Show<MilestoneDetailsModal>("", parameters, Options);
    }

    private static StatusData GetDataByStatus(ProjectStatus status)
    {
        return status switch
        {
            ProjectStatus.Paused => StatusData.Paused,
            ProjectStatus.Open => StatusData.Open,
            ProjectStatus.Closed => StatusData.Closed,
            _ => throw new NotImplementedException("Unknown ProjectStatus type")
        };
    }
    
    private class StatusData(BadgeColor badgeColor, IconName iconName, string title)
    {
        public BadgeColor BadgeColor => badgeColor;
        public IconName IconName => iconName;
        public string Title => title;

        public static StatusData Open => new(BadgeColor.Primary, IconName.ForwardFill, "Открыт");
        public static StatusData Closed => new(BadgeColor.Success, IconName.CheckCircleFill, "Закрыт");
        public static StatusData Paused => new(BadgeColor.Secondary, IconName.PauseCircleFill, "Приостановлен");
    }
}
