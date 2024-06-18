using Cardmngr.Application.Clients;
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
    IRefresheableProjectState,
    IAsyncDisposable
{
    private int previousId = -1;
    private readonly Guid _refreshLocker = Guid.NewGuid();
    private ProjectHubClient hubClient = null!;

    [Parameter] public int Id { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public RefreshService RefreshService { get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private readonly TaskFilterManager taskFilterManager = new();
    public IFilterManager<OnlyofficeTask> TaskFilter => taskFilterManager;

    protected override void OnInitialized()
    {
        RefreshService.Refreshed += OnRefreshModelAsync;
        RefreshService.Start(TimeSpan.FromSeconds(7));
    }

    public async Task ToggleFollowAsync()
    {
        await ProjectClient.FollowProjectAsync(Project.Id);
        Project.IsFollow = !Project.IsFollow;
    }

    private bool _isHubInitialized = false;

    protected override async Task OnParametersSetAsync()
    {
        Initialized = false;
        _isHubInitialized = false;

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

            hubClient = GetNewHubClient();
            await hubClient.StartAsync();

            _isHubInitialized = true;

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

    private async void OnRefreshModelAsync()
    {
        SetModelAsync(await ProjectClient.GetProjectStateAsync(Id)).Forget();
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
        if (hubClient is { })
        {
            await hubClient.DisposeAsync();
        }

        await base.CleanPreviousProjectStateAsync();
        
        TaskFilter.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        if (hubClient is { })
        {
            await hubClient.DisposeAsync();
        }

        RefreshService.Dispose();
    }
}
