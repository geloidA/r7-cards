using Cardmngr.Components.ProjectAggregate.States;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Dashboard;

public partial class MilestonesSummary : KolComponentBase
{
    [CascadingParameter] private IProjectState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.MilestonesChanged += _ => StateHasChanged();
    }
}
