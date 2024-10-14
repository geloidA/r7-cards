using Cardmngr.Application.Clients.Project;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Extensions;

namespace Cardmngr.Components.ProjectAggregate.States;

public sealed partial class MutableProjectState :
    ProjectStateBase,
    IFilterableProjectState,
    IRefreshableProjectState
{
    private int previousId = -1;
    private readonly Guid _refreshLocker = Guid.NewGuid();

    [Parameter] public int Id { get; set; }

    [Parameter] public bool AutoRefresh { get; set; } = true;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] private ITaskClient TaskClient { get; set; } = null!;
    [Inject] private IProjectClient ProjectClient { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public RefreshService RefreshService { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private readonly TaskFilterManager taskFilterManager = new();
    public IFilterManager<OnlyofficeTask> TaskFilter => taskFilterManager;

    protected override void OnInitialized()
    {
        RefreshService.Refreshed += OnRefreshModelAsync;

        if (AutoRefresh)
        {
            RefreshService.Start(TimeSpan.FromSeconds(7));
        }
    }

    public async Task ToggleFollowAsync()
    {
        await ProjectClient.FollowProjectAsync(Project.Id).ConfigureAwait(false);
        Project.IsFollow = !Project.IsFollow;
    }

    protected override async Task OnParametersSetAsync()
    {
        Initialized = false;

        if (previousId != Id)
        {
            RefreshService.Lock(_refreshLocker);

            await CleanPreviousProjectStateAsync();            

            try
            {
                await SetModelAsync(await ProjectClient.GetProjectStateAsync(Id), true);
            }
            catch (OperationCanceledException) 
            {
                RefreshService.RemoveLock(_refreshLocker);
                return;
            }

            previousId = Id;
            RefreshService.RemoveLock(_refreshLocker);
        }
    }

    public override void RemoveMilestone(Milestone milestone)
    {
        base.RemoveMilestone(milestone);

        if (TaskFilter.Filters.SingleOrDefault(x => x is MilestoneTaskFilter) is MilestoneTaskFilter filter)
        {
            filter.Remove(milestone);
        }
    }

    private void OnRefreshModelAsync()
    {
        _ = InvokeAsync(async () =>
        {
            SetModelAsync(await ProjectClient.GetProjectStateAsync(Id)).Forget();
        });
    }

    private ProjectHubClient GetNewHubClient()
    {
        var client = new ProjectHubClient(NavigationManager, TaskClient, Id, AuthenticationStateProvider.ToCookieProvider());

        client.OnUpdatedTask += UpdateTask;
        client.OnDeletedTask += id => RemoveTask(id);
        client.OnCreatedTask += AddTask;

        return client;
    }

    protected override async Task CleanPreviousProjectStateAsync()
    {
        await base.CleanPreviousProjectStateAsync();
        
        TaskFilter.Clear();
    }

    public override void Dispose()
    {
        base.Dispose();
        RefreshService.Dispose();
    }
}
