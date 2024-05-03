using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Blazored.Modal.Services;
using Blazored.Modal;
using Cardmngr.Components.Modals.ConfirmModals;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Extensions;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard : ComponentBase
{
    [Inject] public ITaskClient TaskClient { get; set; } = null!;

    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] ProjectHubClient? ProjectHubClient { get; set; }
    [CascadingParameter] IModalService Modal { get; set; } = default!;
    [CascadingParameter(Name = "MiddleModal")] ModalOptions ModalOptions { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.TaskFilter.FilterChanged += StateHasChanged;
        State.TasksChanged += StateHasChanged;
        State.StateChanged += StateHasChanged;
        State.MilestonesChanged += StateHasChanged;
    }

    private ICollection<OnlyofficeTask> GetTasksForStatus(OnlyofficeTaskStatus status)
    {
        var filtered = State
            .FilteredTasks()
            .FilterByStatus(status);

        return status.StatusType == StatusType.Open
            ? [.. filtered.OrderByTaskCriteria()]
            : [.. filtered.OrderByDescending(x => x.Updated)];
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

            ProjectHubClient?.SendUpdatedTaskAsync(task.ProjectOwner.Id, task.Id).Forget();
        }
    }
}
