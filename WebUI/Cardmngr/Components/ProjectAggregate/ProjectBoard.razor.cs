using System.Collections.Concurrent;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard : ComponentBase
{
    private readonly ConcurrentDictionary<int, List<TaskTag>> _tagsByTaskId = new();

    [CascadingParameter] IProjectState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.TaskFilter.FilterChanged += StateHasChanged;
        State.TasksChanged += StateHasChanged;
        State.StateChanged += StateHasChanged;
        State.MilestonesChanged += StateHasChanged;
    }
}
