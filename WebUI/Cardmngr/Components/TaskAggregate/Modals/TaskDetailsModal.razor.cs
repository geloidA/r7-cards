using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Shared.Extensions;
using Cardmngr.Notification;
using Cardmngr.Services;
using Microsoft.FluentUI.AspNetCore.Components;
using Cardmngr.Shared.Utils.Comparer;
using Cardmngr.Application.Clients.Subtask;
using Blazored.Modal.Services;
using Cardmngr.Components.Modals.ConfirmModals;
using Cardmngr.Utils;
using Cardmngr.Shared.Utils;
using Cardmngr.Components.TaskAggregate.ModalComponents;

namespace Cardmngr.Components.TaskAggregate.Modals;

public sealed partial class TaskDetailsModal() : 
    AddEditModalBase<OnlyofficeTask, TaskUpdateData>(new OnlyofficeTaskUpdateDataEqualityComparer()), 
    IDisposable
{
    private TaskTagLabels _taskTagsLabels = null!;

    private bool _isDescriptionEditting;
    private bool _showSubtasks;
    private bool _isSubtasksOpen;

    private readonly Guid lockGuid = Guid.NewGuid();
    private Components.Modals.MyBlazored.Offcanvas currentModal = null!;
    private bool CanEdit => !State.ReadOnly && (Model == null || (!Model.IsClosed() && Model.CanEdit));

    [Inject] private ITaskClient TaskClient { get; set; } = null!;
    [Inject] private ITagColorManager TagColorGetter { get; set; } = null!;
    [Inject] private NotificationHubConnection NotificationHubConnection { get; set; } = null!;
    [Inject] private ISubtaskClient SubtaskClient { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;

    [Parameter] public IProjectState State { get; set; } = null!;
    [Parameter] public int TaskStatusId { get; set; }

    protected override bool CanBeSaved => !_isDescriptionEditting;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (State is IRefreshableProjectState refreshableProjectState)
        {
            refreshableProjectState.RefreshService.Lock(lockGuid);
        }

        if (IsAdd)
        {
            Buffer.Title = "Новая задача";
            Buffer.TaskStatusId = TaskStatusId;
        }
    }

    private bool submitting;

    private async Task SubmitAsync()
    {
        if (enterPressed)
        {
            enterPressed = false;
            return;
        }

        submitting = true;
        if (IsAdd)
        {
            await Catcher.CatchAsync<HttpRequestException>(AddTaskAsync, ex => ToastService.ShowError(ex.Message));
        }
        else
        {
            var canceled = await Catcher.CatchAsync<HttpRequestException, bool>(EditTaskAsync, ex => ToastService.ShowError(ex.Message), true);
            if (canceled)
            {
                submitting = false;
                return;
            }
        }

        SkipConfirmation = true;
        await currentModal.CloseAsync(ModalResult.Ok(IsAdd ? ModalResultType.Added : ModalResultType.Edited)).ConfigureAwait(false);
    }

    private async Task AddTaskAsync()
    {        
        var created = await TaskClient.CreateAsync(State.Project.Id, Buffer).ConfigureAwait(false);
            
        var tagsTasks = _taskTagsLabels.TaskTags.Select(x => TaskClient.CreateTagAsync(created.Id, x.Name));
        var tags = await Task.WhenAll(tagsTasks).ConfigureAwait(false);

        created.Tags = [.. tags];

        State.AddTask(created);
        NotificationHubConnection.NotifyAboutCreatedTaskAsync(created).Forget();
    }

    private async Task<bool> EditTaskAsync()
    {
        var bufferTask = (IOnlyofficeTask)Buffer;
        var selectedStatus = State.Statuses.SingleOrDefault(x => x.Id == bufferTask.TaskStatusId) ?? State.DefaultStatus(bufferTask.Status);

        if (selectedStatus.StatusType == Domain.Enums.StatusType.Close && Model!.HasUnclosedSubtask())
        {
            var confirmModal = await Modal.Show<CloseCardConfirmModal>(MiddleModal).Result;
            if (confirmModal.Cancelled) return true;
        }

        Buffer.Status = null; // Когда сохраняется задача, с ненулевым статусом, онлиофис устанавливает статус по умолчанию для данного типа статуса.

        if (Model!.TaskStatusId != Buffer.TaskStatusId)
        {
            await TaskClient.UpdateTaskStatusAsync(Buffer.Id, selectedStatus).ConfigureAwait(false);
        }
        
        var updated = await TaskClient.UpdateAsync(Model.Id, Buffer).ConfigureAwait(false);
        State.UpdateTask(updated);

        return false;
    }

    public void StartSubtaskAdding()
    {
        _isSubtasksOpen = true;
        _showSubtasks = true;
    }

    private async Task DeleteAsync()
    {
        var answer = await ShowDeleteConfirm("Удалить задачу?").ConfigureAwait(false);
        
        if (answer.Confirmed)
        {
            try
            {
                await TaskClient.RemoveAsync(Model!.Id).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                ToastService.ShowError(ex.Message);
                return;
            }
            
            foreach (var tag in Model.Tags)
                TagColorGetter.RemoveTag(tag);

            State.RemoveTask(Model);

            SkipConfirmation = true;
            await currentModal.CloseAsync(ModalResult.Ok(ModalResultType.Deleted)).ConfigureAwait(false);
        }
    }

    private async Task DeleteAllSubtasksAsync()
    {        
        var answer = await ShowDeleteConfirm("Удалить все подзадачи?").ConfigureAwait(false);

        if (answer.Confirmed)
        {
            try
            {
                var deleteTasks = Model!.Subtasks.Select(x => SubtaskClient.RemoveAsync(Model.Id, x.Id));
                await Task.WhenAll(deleteTasks).ConfigureAwait(false);

                foreach (var tag in Model.Subtasks.ToList())
                {
                    State.RemoveSubtask(Model.Id, tag.Id);
                }
            }
            catch (HttpRequestException ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }
    }

    public void Dispose()
    {
        if (State is IRefreshableProjectState { RefreshService.Disposed: false } refreshableProjects)
        {
            refreshableProjects.RefreshService.RemoveLock(lockGuid);
        }
    }
}
