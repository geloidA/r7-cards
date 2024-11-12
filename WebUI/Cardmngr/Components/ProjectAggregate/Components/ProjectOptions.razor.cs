using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class ProjectOptions
{
    private string _boardUri = null!;
    private string _ganttUri = null!;

    [CascadingParameter] private IProjectState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        _boardUri = $"project/board?ProjectId={State.Project.Id}";
        _ganttUri = $"project/gantt?ProjectId={State.Project.Id}";
    }
}
