using Cardmngr.Application.Clients;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Utils;
using KolBlazor.Extensions;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public partial class DashboardProjectState : 
    ProjectStateBase, 
    IFilterableProjectState,
    IRefresheableProjectState
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
        RefreshService.Refreshed += OnRefreshModelAsync;
        RefreshService.Start(TimeSpan.FromMinutes(1));
    }

    private int _previousId = -1;

    protected override async Task OnParametersSetAsync()
    {
        Initialized = false;

        if (_previousId != Id)
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

            _previousId = Id;
            RefreshService.RemoveLock(_refreshLocker);
        }
    }

    protected override Task CleanPreviousProjectStateAsync()
    {
        TaskFilter.Clear();
        return base.CleanPreviousProjectStateAsync();
    }

    private async void OnRefreshModelAsync()
    {
        SetModelAsync(await ProjectClient.GetProjectStateAsync(Id)).Forget();
    }

    public override void Dispose()
    {
        RefreshService.Dispose();
        base.Dispose();
    }
}
