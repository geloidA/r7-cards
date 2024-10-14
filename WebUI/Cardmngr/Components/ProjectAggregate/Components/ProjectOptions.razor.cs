using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class ProjectOptions
{
    private bool _popoverOpen = false;

    [CascadingParameter] private IProjectState State { get; set; } = null!;
}
