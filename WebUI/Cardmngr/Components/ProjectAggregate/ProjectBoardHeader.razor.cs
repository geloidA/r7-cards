using Microsoft.AspNetCore.Components;
using Cardmngr.Components.ProjectAggregate.States;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoardHeader : ComponentBase
{
    private bool collapsed;
    [CascadingParameter] IProjectState State { get; set; } = null!;
}
