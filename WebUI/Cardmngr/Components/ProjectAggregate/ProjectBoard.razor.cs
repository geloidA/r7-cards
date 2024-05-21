using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard : ComponentBase
{
    [CascadingParameter] IProjectState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.TaskFilter.FilterChanged += StateHasChanged;
        State.TasksChanged += StateHasChanged;
        State.MilestonesChanged += StateHasChanged;
    }
}
