using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Components.Modals.ConfirmModals;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using KolBlazor.Components;
using Cardmngr.Services;
using System.Collections.Concurrent;

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class StatusColumn : ComponentBase
{
    private bool isStateChangedFollowed;
    private KolBoardColumn<OnlyofficeTask> column = null!;

    [Inject] 
    private ITaskClient TaskClient { get; set; } = null!;

    [Parameter, EditorRequired]
    public OnlyofficeTaskStatus Status { get; set; } = null!;
    
    [CascadingParameter] 
    private IProjectState State { get; set; } = null!;

    [CascadingParameter] 
    ProjectHubClient? ProjectHubClient { get; set; }

    [CascadingParameter] 
    IModalService Modal { get; set; } = default!;

    [CascadingParameter(Name = "MiddleModal")] 
    ModalOptions ModalOptions { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !isStateChangedFollowed)
        {
            isStateChangedFollowed = true;
            State.TasksChanged += () => column.RefreshDataAsync().Forget();
            await Task.Delay(500); // wait for tasks tags loaded. TODO: remove
            column.RefreshDataAsync().Forget();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private IList<OnlyofficeTask> GetTasksForStatus(OnlyofficeTaskStatus status)
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
    
    [CascadingParameter] ConcurrentDictionary<int, List<TaskTag>> TagsByTaskId { get; set; } = null!;

    private int GetTaskHeight(OnlyofficeTask task)
    {
        TagsByTaskId.TryGetValue(task.Id, out var tagsByTaskId);
        return ItemHeightCalculator.CalculateHeight(task, tagsByTaskId);
    }
}
