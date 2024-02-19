using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Blazored.Modal.Services;
using Blazored.Modal;
using Cardmngr.Components.Modals.ConfirmModals;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard
{
    [CascadingParameter] ProjectState State { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = default!;
    [CascadingParameter(Name = "MiddleModal")] ModalOptions ModalOptions { get; set; } = null!;

    private async Task OnChangeTaskStatus(OnlyofficeTask task, OnlyofficeTaskStatus status)
    {
        if (status.StatusType == StatusType.Close && task.HasUnclosedSubtask())
        {
            var confirmModal = await Modal.Show<CloseCardConfirmModal>("Закрытие задачи", ModalOptions).Result;
            if (confirmModal.Cancelled) return;
            task.CloseAllSubtasks();
        }

        await State.UpdateTaskStatusAsync(task, status);
    }
}
