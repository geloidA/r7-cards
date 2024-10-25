using Cardmngr.Application.Clients.Project;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public partial class DashboardProjectState : 
    ProjectStateComponentBase, 
    IFilterableProjectState,
    IRefreshableProjectState,
    IDisposable
{
    public DashboardProjectState() : base(isReadOnly: true)
    {
    }

    [Inject] IProjectClient ProjectClient { get; set; } = null!;

    [Inject] public RefreshService RefreshService { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private readonly IFilterManager<OnlyofficeTask> _taskFilter = new TaskFilterManager();
    private readonly Guid _refreshLocker = Guid.NewGuid();

    public IFilterManager<OnlyofficeTask> TaskFilter => _taskFilter;

    [Parameter] public int Id { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        RefreshService.Refreshed += OnRefreshModelAsync;
        RefreshService.Start(TimeSpan.FromMinutes(1));
    }

    private int _previousId = -1;

    protected override void OnParametersSet()
    {
        throw new NotImplementedException();

        // Initialized = false;

        // if (_previousId != Id)
        // {
        //     RefreshService.Lock(_refreshLocker);

        //     await CleanPreviousProjectStateAsync();

        //     try
        //     {
        //         SetModel(await ProjectClient.GetProjectStateAsync(Id));
        //     }            
        //     catch (OperationCanceledException) 
        //     {
                
        //     }

        //     _previousId = Id;
        //     RefreshService.RemoveLock(_refreshLocker);
        // }
    }

    protected override Task CleanPreviousProjectStateAsync()
    {
        TaskFilter.Clear();
        return base.CleanPreviousProjectStateAsync();
    }

    private void OnRefreshModelAsync()
    {
        _ = InvokeAsync(async () =>
        {
            SetModel(await ProjectClient.GetProjectStateAsync(Id));
        });
    }

    public void Dispose()
    {
        RefreshService.Dispose();
    }
}
