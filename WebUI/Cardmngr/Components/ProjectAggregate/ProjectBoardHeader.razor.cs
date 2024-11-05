using Microsoft.AspNetCore.Components;
using Cardmngr.Components.ProjectAggregate.States;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoardHeader : ComponentBase
{
    [CascadingParameter] IProjectState State { get; set; } = null!;
}
