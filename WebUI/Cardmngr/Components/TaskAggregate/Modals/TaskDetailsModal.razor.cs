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

namespace Cardmngr.Components.TaskAggregate.Modals;

public sealed partial class TaskDetailsModal() : AddEditModalBase<OnlyofficeTask, TaskUpdateData>(new OnlyofficeTaskUpdateDataEqualityComparer()), 
    IDisposable
{
    private bool _isDescriptionEditting;
    private bool _showSubtasks;
    private bool _isSubtasksOpen;

    private readonly Guid lockGuid = Guid.NewGuid();
    private Components.Modals.MyBlazored.Offcanvas currentModal = null!;
    private bool CanEdit => !State.ReadOnly && (Model == null || Model.CanEdit);

    [Inject] private ITaskClient TaskClient { get; set; } = null!;
    [Inject] private ITagColorManager TagColorGetter { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    [Inject] private NotificationHubConnection NotificationHubConnection { get; set; } = null!;
    [Inject] private ISubtaskClient SubtaskClient { get; set; } = null!;

    [Parameter] public IProjectState State { get; set; } = null!;
    [Parameter] public int TaskStatusId { get; set; }
    [Parameter] public List<TaskTag> TaskTags { get; set; } = [];

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

    private void NotifyThatTaskWasChanged(OnlyofficeTask upd)
    {
        if (upd.Id != Model!.Id) return;
        ToastService.ShowInfo("Задача была изменена кем-то другим");

        SkipConfirmation = true;
        currentModal.CloseAsync().Forget();
    }

    private void NotifyThatTaskWasDeleted(int taskId)
    {
        if (taskId != Model?.Id) return;
        ToastService.ShowInfo("Задача была удалена кем-то другим");

        SkipConfirmation = true;
        currentModal.CloseAsync().Forget();
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
            var created = await TaskClient.CreateAsync(State.Project.Id, Buffer).ConfigureAwait(false);
            
            var tagsTasks = TaskTags.Select(x => TaskClient.CreateTagAsync(created.Id, x.Name));
            var tags = await Task.WhenAll(tagsTasks).ConfigureAwait(false);

            created.Tags = [.. tags];

            State.AddTask(created);
            NotificationHubConnection.NotifyAboutCreatedTaskAsync(created).Forget();
        }
        else
        {
            var updated = await TaskClient.UpdateAsync(Model!.Id, Buffer).ConfigureAwait(false);
            State.UpdateTask(updated);
        }

        SkipConfirmation = true;
        await currentModal.CloseAsync().ConfigureAwait(false);
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
            foreach (var tag in TaskTags)
                TagColorGetter.RemoveTag(tag);

            await TaskClient.RemoveAsync(Model!.Id).ConfigureAwait(false);
            State.RemoveTask(Model);

            SkipConfirmation = true;
            await currentModal.CloseAsync().ConfigureAwait(false);
        }
    }

    private async Task DeleteAllSubtasksAsync()
    {        
        var answer = await ShowDeleteConfirm("Удалить все подзадачи?").ConfigureAwait(false);

        if (answer.Confirmed)
        {
            var deleteTasks = Model!.Subtasks
                .ToList()
                .Select(x => 
                {
                    State.RemoveSubtask(Model.Id, x.Id);
                    return SubtaskClient.RemoveAsync(Model.Id, x.Id);
                });

            await Task.WhenAll(deleteTasks).ConfigureAwait(false);
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
