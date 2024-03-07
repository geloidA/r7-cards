using Cardmngr.Application.Clients;
using Cardmngr.Pages;
using Cardmngr.Shared.Project;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class StaticProjectState : ProjectStateBase, IRefresheableProjectState, IDisposable // TODO: Rename
{
    private readonly Guid lockGuid = Guid.NewGuid();
    private bool isCollapsed = true;

    private string CssHeight => isCollapsed 
        ? "min-height: 50px;" 
        : "max-height: 850px; min-height: 850px;";

    [Parameter] public ProjectStateVm? ViewModel { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [CascadingParameter(Name = "UserId")] string? UserId { get; set; }
    [CascadingParameter] SelfTasksPage SelfTasksPage { get; set; } = null!;

    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] public RefreshService RefreshService { get; set; } = null!;

    private bool IsCurrentUserInTeam => Model is { } && Model.Team.Any(x => x.Id == UserId);

    protected override void OnInitialized()
    {
        if (ViewModel is { })
        {
            Model = ViewModel;
            Initialized = true;
            TasksChanged += DisposeIfEmpty;
            RefreshService.Refreshed += OnRefreshModel;
        }
    }

    private async void OnRefreshModel()
    {
        Initialized = false;
        if (Model?.Project is { })
        {
            var state = await ProjectClient.GetProjectAsync(Model.Project.Id);
            state.Tasks.RemoveAll(x => x.Responsibles.All(y => y.Id != UserId));

            DisposeIfEmpty(state);

            Initialized = true;
            Model = state;
        }
    }

    private void DisposeIfEmpty() => DisposeIfEmpty(Model);

    private void DisposeIfEmpty(ProjectStateVm? state)
    {
        if (state?.Tasks.Count == 0)
        {
            SelfTasksPage.RemoveProject(state.Project?.Id);
            Dispose();
        }
    }

    private void ToggleCollapsed()
    {
        isCollapsed = !isCollapsed;

        if (isCollapsed)
        {
            RefreshService.Lock(lockGuid);
        }
        else
        {
            if (!RefreshService.Started)
            {
                RefreshService.Start(TimeSpan.FromSeconds(5));
            }

            RefreshService.RemoveLock(lockGuid);
        }
    }

    public void Dispose()
    {
        RefreshService.Dispose();
    }
}
