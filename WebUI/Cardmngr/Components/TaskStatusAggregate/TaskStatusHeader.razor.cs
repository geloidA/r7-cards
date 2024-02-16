using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.Modals.DetailModals;

namespace Cardmngr.Components.TaskStatusAggregate;

public partial class TaskStatusHeader
{
    [CascadingParameter] ProjectState State { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")] ModalOptions DetailsOptions { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = default!;

    [Parameter] public OnlyofficeTaskStatus TaskStatus { get; set; } = null!;

    [Parameter] public bool IsCollapsed { get; set; }

    public int TaskCount => State.Model?.Tasks.FilterByStatus(TaskStatus).Count() ?? 0;

    private async Task ShowCreateTaskModal()
    {
        var parameters = new ModalParameters
        {
            // { "Model", new TaskModel(new MyTask { CanEdit = true }, StatusColumn) },
            // { "IsCreation", true },
            // { "IsFeedback", IsFeedback },
            // { "ProjectApi", ProjectApi }
        };

        await Modal.Show<TaskDetailsModal>("", parameters, DetailsOptions).Result;
    }

}
