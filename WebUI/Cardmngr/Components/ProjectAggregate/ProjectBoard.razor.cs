using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard
{
    [CascadingParameter] ProjectState State { get; set; } = null!;

    private async Task OnChangeTaskStatus(OnlyofficeTask task, OnlyofficeTaskStatus status)
    {
        await State.UpdateTaskStatusAsync(task, status);
    }
}
