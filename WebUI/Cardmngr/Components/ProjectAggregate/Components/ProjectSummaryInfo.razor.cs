using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using System.Diagnostics;

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class ProjectSummaryInfo : ComponentBase
{
    private ICollection<OnlyofficeTask> _deadlineoutTasks = null!;
    private ICollection<OnlyofficeTask> _deadlineoutSoonTasks = null!;
    private IRefresheableProjectState? _refreshableState;

    [CascadingParameter] IProjectState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (State is IRefresheableProjectState refresheable)
        {
            _refreshableState = refresheable;
            _refreshableState.RefreshService.Refreshed += RefreshState;
        }
        
        RefreshState();
    }

    private void RefreshState()
    {
        _deadlineoutTasks = [.. State.Tasks.Where(x => x.IsDeadlineOut())];
        _deadlineoutSoonTasks = [.. State.Tasks.Where(x => !x.IsDeadlineOut() && x.IsSevenDaysDeadlineOut())];
        StateHasChanged();
    }
}
