using Cardmngr.Application.Clients.Project;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public sealed partial class MutableProjectState :
    ProjectStateComponentBase,
    IFilterableProjectState,
    IRefreshableProjectState
{
    private int _lastId = -1;
    private readonly Dictionary<int, int> _commonHeightByKey = [];

    private readonly Guid _refreshLocker = Guid.NewGuid();

    [Parameter] public int Id { get; set; } = -1;

    [Parameter] public bool AutoRefresh { get; set; } = true;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] private IProjectClient ProjectClient { get; set; } = null!;
    [Inject] public RefreshService RefreshService { get; set; } = null!;

    private readonly TaskFilterManager taskFilterManager = new();
    public IFilterManager<OnlyofficeTask> TaskFilter => taskFilterManager;

    protected override void OnInitialized()
    {
        base.OnInitialized();

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
        if (Id == -1 || _lastId == Id) return;

        _lastId = Id;

        Initialized = false;        

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
        catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            
        }

        RefreshService.RemoveLock(_refreshLocker);

        Initialized = true;
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
        InvokeAsync(async () =>
        {
            SetModelAsync(await ProjectClient.GetProjectStateAsync(Id)).Forget();
        });
    }

    protected override async Task CleanPreviousProjectStateAsync()
    {
        await base.CleanPreviousProjectStateAsync();

        _commonHeightByKey.Clear();        
        TaskFilter.Clear();
    }

    public override void Dispose()
    {
        base.Dispose();
        RefreshService.Dispose();
    }
}
