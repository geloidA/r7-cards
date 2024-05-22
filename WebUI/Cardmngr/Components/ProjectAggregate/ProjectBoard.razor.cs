using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard : ComponentBase
{
    private readonly Dictionary<object, int> _commonHeightByKey = []; // TODO: move to specific class

    [CascadingParameter] IProjectState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.TasksChanged += e =>
        {
            if (e is { Action: Models.TaskAction.Update, Task: { } })
            {
                _commonHeightByKey.Remove(e.Task.Id);
            }
        };

        base.OnInitialized();
    }
}
