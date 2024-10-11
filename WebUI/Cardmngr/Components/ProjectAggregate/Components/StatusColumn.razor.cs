using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Components.Modals.ConfirmModals;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class StatusColumn : ComponentBase, IDisposable
{
    private IList<OnlyofficeTask> _tasks = [];

    [Inject] 
    private ITaskClient TaskClient { get; set; } = null!;

    [Inject]
    private IToastService ToastService { get; set; } = null!;

    [Parameter, EditorRequired]
    public OnlyofficeTaskStatus Status { get; set; } = null!;
    
    [CascadingParameter] 
    private IProjectState State { get; set; } = null!;

    [CascadingParameter] private IModalService Modal { get; set; } = default!;

    [CascadingParameter(Name = "MiddleModal")]
    private ModalOptions ModalOptions { get; set; } = null!;

    [Parameter]
    public Dictionary<int, int>? CommonHeightByKey { get; set; }

    protected override void OnInitialized()
    {
        _tasks = GetTasksForStatus(State, Status);
        State.TasksChanged += RefreshColumn;

        if (State is IFilterableProjectState filterableState)
        {
            filterableState.TaskFilter.FilterChanged += () => RefreshColumn(null);
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Console.WriteLine("OnParametersSet");
    }

    private void RefreshColumn(EntityChangedEventArgs<OnlyofficeTask>? args)
    {
        _tasks = GetTasksForStatus(State, Status);
        StateHasChanged();
    }

    private static IList<OnlyofficeTask> GetTasksForStatus(IProjectState state, OnlyofficeTaskStatus status)
    {
        IEnumerable<OnlyofficeTask> tasks = state.Tasks;

        if (state is IFilterableProjectState filterableState)
        {
            tasks = filterableState.FilteredTasks();
        }
        
        return [.. tasks
            .FilterByStatus(status)
            .OrderByTaskCriteria()];
    }

    private async Task OnTaskDropped(OnlyofficeTask task)
    {
        if (Status.StatusType == StatusType.Close && task.HasUnclosedSubtask())
        {
            var confirmModal = await Modal.Show<CloseCardConfirmModal>(ModalOptions).Result;
            if (confirmModal.Cancelled) return;
        }

        if (!task.HasStatus(Status))
        {
            try 
            {
                var updated = await TaskClient.UpdateTaskStatusAsync(task.Id, Status);
                State.ChangeTaskStatus(updated);
            }
            catch (HttpRequestException e)
            {
                ToastService.ShowError(e.HttpRequestError.ToString());
            }
        }
    }

    public void Dispose()
    {
        State.TasksChanged -= RefreshColumn;
    }
}
