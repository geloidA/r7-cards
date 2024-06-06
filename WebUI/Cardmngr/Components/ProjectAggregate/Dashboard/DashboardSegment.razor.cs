using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Dashboard;

public partial class DashboardSegment : KolComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
