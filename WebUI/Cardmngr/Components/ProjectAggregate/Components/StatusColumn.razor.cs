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
using Cardmngr.Components.ProjectAggregate.Models;

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class StatusColumn : ComponentBase, IDisposable
{
    private KolBoardColumn<OnlyofficeTask> boardColumnComponent = null!;
    private IList<OnlyofficeTask> _tasks = [];

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

    [Parameter]
    public Dictionary<int, int>? CommonHeightByKey { get; set; }

    protected override void OnInitialized()
    {
        _tasks = GetTasksForStatus(State, Status);
        State.TasksChanged += RefreshColumn;
        State.TaskFilter.FilterChanged += () => RefreshColumn(null);
        
        base.OnInitialized();
    }

    private void RefreshColumn(TaskChangedEventArgs? args)
    {
        _tasks = GetTasksForStatus(State, Status);
        StateHasChanged();
    }

    private static IList<OnlyofficeTask> GetTasksForStatus(IProjectState state, OnlyofficeTaskStatus status)
    {
        return [.. state.FilteredTasks()
            .FilterByStatus(status)
            .OrderByTaskCriteria()];
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
            State.ChangeTaskStatus(updated);

            ProjectHubClient?.SendUpdatedTaskAsync(task.ProjectOwner.Id, task.Id).Forget();
        }
    }

    public void Dispose()
    {
        State.TasksChanged -= RefreshColumn;
    }
}
