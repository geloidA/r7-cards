
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
    public IFilterManager<OnlyofficeTask> TaskFilter => _taskFilter;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await SetModelAsync(await ProjectClient.GetProjectAsync(Id), true);
        RefreshService.Refreshed += OnRefreshModelAsync;
        RefreshService.Start(TimeSpan.FromMinutes(1));
    }

    private async void OnRefreshModelAsync()
    {
        SetModelAsync(await ProjectClient.GetProjectAsync(Id)).Forget();
    }
}
