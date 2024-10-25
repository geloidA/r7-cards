using Cardmngr.Application.Clients.Project;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public sealed partial class StaticProjectStateView : ComponentBase
{
    private string CssHeight => ViewModel.IsCollapsed
        ? "height: auto;"
        : "height: 650px;";

    private Icon CollapsedIcon => ViewModel.IsCollapsed
        ? new Icons.Regular.Size16.ChevronDown()
        : new Icons.Regular.Size16.ChevronUp();

    [Parameter] public StaticProjectVm ViewModel { get; set; } = null!;
    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] AppProjectsInfoService ProjectSummaryService { get; set; } = null!;

    protected override void OnInitialized()
    {
        toggleCollapsedFunc = ToggleCollapsed;
        isFollow = ProjectSummaryService.IsFollow(ViewModel.Project.Id);
    }

    private Action toggleCollapsedFunc = null!;

    private void ToggleCollapsed()
    {
        ViewModel.ToggleCollapsed(TaskClient);
    }

    private bool isFollow;

    private Icon IconFollow => isFollow ? new Icons.Filled.Size16.Star() : new Icons.Regular.Size16.Star();

    public async Task ToggleFollowAsync()
    {
        await ProjectClient.FollowProjectAsync(ViewModel.Project.Id);

        if (isFollow = !isFollow)
        {
            ProjectSummaryService.Follow(ViewModel.Project.Id);
        }
        else
        {
            ProjectSummaryService.Unfollow(ViewModel.Project.Id);
        }
    }
}
