using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Blazored.Modal.Services;
using Blazored.Modal;
using Cardmngr.Components.Modals.ConfirmModals;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.SignalRHubClients;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard
{
    [Inject] public ITaskClient TaskClient { get; set; } = null!;

    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] ProjectHubClient ProjectHubClient { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = default!;
    [CascadingParameter(Name = "MiddleModal")] ModalOptions ModalOptions { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.SelectedMilestonesChanged += StateHasChanged;
        State.TasksChanged += StateHasChanged;
        State.StateChanged += StateHasChanged;
        State.MilestonesChanged += StateHasChanged;
    }

    private async Task OnChangeTaskStatus(OnlyofficeTask task, OnlyofficeTaskStatus status)
    {
        if (status.StatusType == StatusType.Close && task.HasUnclosedSubtask())
        {
            var confirmModal = await Modal.Show<CloseCardConfirmModal>("Закрытие задачи", ModalOptions).Result;
            if (confirmModal.Cancelled) return;
        }

        if (!task.HasStatus(status))
        {
            var updated = await TaskClient.UpdateTaskStatusAsync(task.Id, status);
            State.UpdateTask(updated);

            await ProjectHubClient.SendUpdatedTaskAsync(State.Model!.Project!.Id, task.Id);
        }
    }
}
