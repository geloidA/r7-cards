using Cardmngr.Application.Clients;
using Cardmngr.Components.ProjectAggregate.Vms;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Cardmngr.Shared;
using Cardmngr.Shared.Utils.Filter;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class StaticProjectState : ProjectStateBase, IProjectState, IDisposable
{
    private string CssHeight => ViewModel?.IsCollapsed ?? true 
        ? "min-height: 50px;" 
        : "max-height: 650px; min-height: 650px;";

    private Icon CollapsedIcon => ViewModel?.IsCollapsed ?? true 
        ? new Icons.Regular.Size16.ChevronDown() 
        : new Icons.Regular.Size16.ChevronUp();

    [Parameter] public StaticProjectVm? ViewModel { get; set; }
    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] ProjectSummaryService ProjectSummaryService { get; set; } = null!;
    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        toggleCollapsedFunc = ToggleCollapsed;
        if (ViewModel is { IsCollapsed: false })
        {
            Model = await ProjectClient.CreateProjectWithTasksAsync(ViewModel.Tasks);
            Initialized = true;
        }

        isFollow = ProjectSummaryService.FollowedProjectIds.Contains(ViewModel?.ProjectInfo.Id ?? -1);

        SummaryService.OnProjectsChanged += ToggleIfSelected;
    }

    private async void ToggleIfSelected()
    {
        await Task.Delay(5); // wait for correct toggling
        if (ViewModel is { } && SummaryService.FilterManager.ProjectId == ViewModel.ProjectInfo.Id)
        {
            await ToggleCollapsed();
            StateHasChanged();
        }
    }

    private readonly TaskFilterManager taskFilterManager = new();
    public override IFilterManager<OnlyofficeTask> TaskFilter => taskFilterManager;

    private Func<Task> toggleCollapsedFunc = null!;

    private async Task ToggleCollapsed()
    {
        if (ViewModel is { })
        {
            ViewModel.IsCollapsed = !ViewModel.IsCollapsed;

            Model = ViewModel.IsCollapsed ? null : await ProjectClient.CreateProjectWithTasksAsync(ViewModel.Tasks);
            Initialized = Model is { };
        }
    }

    private bool isFollow;

    private Icon IconFollow => isFollow ? new Icons.Filled.Size16.Star() : new Icons.Regular.Size16.Star();

    public async Task ToggleFollowAsync()
    {
        if (ViewModel?.ProjectInfo is { })
        {
            await ProjectClient.FollowProjectAsync(ViewModel.ProjectInfo.Id);
            
            if (isFollow = !isFollow)
            {
                ProjectSummaryService.FollowedProjectIds.Add(ViewModel.ProjectInfo.Id);
            }
            else
            {
                ProjectSummaryService.FollowedProjectIds.Remove(ViewModel.ProjectInfo.Id);
            }
        }
    }

    public void Dispose()
    {
        SummaryService.OnProjectsChanged -= ToggleIfSelected;
    }
}
