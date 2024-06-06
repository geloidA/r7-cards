using Cardmngr.Components.ProjectAggregate.States;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Dashboard;

public partial class MilestonesSummary : KolComponentBase
{
    [CascadingParameter] IProjectState State { get; set; } = null!;
}
