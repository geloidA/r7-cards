using Cardmngr.Application.Clients.Project;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public sealed partial class MutableProjectState :
    ProjectStateComponentBase,
    IFilterableProjectState,
    IRefreshableProjectState,
    IDisposable
{
    private int _lastId = -1;
    private bool _notFound;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly Dictionary<int, int> _commonHeightByKey = [];

    private readonly Guid _refreshLocker = Guid.NewGuid();

    [Parameter] public int Id { get; set; } = -1;

    [Parameter] public bool AutoRefresh { get; set; } = true;
    [Parameter] public bool SilentTagInitialized { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] private IProjectClient ProjectClient { get; set; } = null!;
    [Inject] private ITaskClient TaskClient { get; set; } = null!;
    [Inject] public RefreshService RefreshService { get; set; } = null!;

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

    protected override async Task OnParametersSetAsync()
    {
        if (Id == -1 || _lastId == Id) return;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new();

        _lastId = Id;

        Initialized = false;

        RefreshService.Lock(_refreshLocker);

        await CleanPreviousProjectStateAsync();
        
        try
        {
            SetModel(await ProjectClient.GetProjectStateAsync(Id));
        }
        catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _notFound = true;
            return;
        }

        Initialized = true;
        StateHasChanged();

        try
        {
            await InitializeTaskTagsAsync(TaskClient, SilentTagInitialized, _cancellationTokenSource.Token);
        }
        catch (OperationCanceledException) 
        {
            Console.WriteLine("Tags initialization canceled");
        }
        catch (ObjectDisposedException e)
        {
            Console.WriteLine(e.Message);
        }

        RefreshService.RemoveLock(_refreshLocker);
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
        InvokeAsync(async () => SetModel(await ProjectClient.GetProjectStateAsync(Id)));
    }

    protected override async Task CleanPreviousProjectStateAsync()
    {
        await base.CleanPreviousProjectStateAsync();

        _commonHeightByKey.Clear();        
        TaskFilter.Clear();
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
        RefreshService.Dispose();
    }
}
