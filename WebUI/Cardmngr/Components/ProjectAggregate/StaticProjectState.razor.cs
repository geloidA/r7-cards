using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class StaticProjectState : ProjectStateBase // TODO: Rename
{
    [Parameter] public ProjectStateVm? State { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        if (State is { })
        {
            Model = State;
            Initialized = true;
        }
    }
}
