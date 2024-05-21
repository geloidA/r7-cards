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

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class StatusColumn : ComponentBase, IDisposable
{
    private KolBoardColumn<OnlyofficeTask> boardColumnComponent = null!;

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

    private int _opacity;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Delay(1);
            _opacity = 1;
            State.TasksChanged += RefreshColumn;
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void RefreshColumn()
    {
        boardColumnComponent.RefreshDataAsync();
    }

    protected override void OnInitialized()
    {
        State.TasksChanged += StateHasChanged;
        base.OnInitialized();
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
            State.UpdateTask(updated);

            ProjectHubClient?.SendUpdatedTaskAsync(task.ProjectOwner.Id, task.Id).Forget();
        }
    }

    public void Dispose()
    {
        State.TasksChanged -= RefreshColumn;
    }
}
