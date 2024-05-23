using Cardmngr.Services;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Filter;

public partial class ProjectFilterToolbar : ComponentBase
{
    private readonly IEnumerable<TaskSelectorType> _taskSelectorTypes = Enum
        .GetValues(typeof(TaskSelectorType))
        .Cast<TaskSelectorType>();

    [CascadingParameter] IProjectState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        
    }

    public TaskSelectorType SelectorType { get; set; }
}
