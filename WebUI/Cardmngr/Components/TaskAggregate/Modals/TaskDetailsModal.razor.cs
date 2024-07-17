using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Shared.Extensions;
using Cardmngr.Notification;
using Cardmngr.Services;
using Microsoft.FluentUI.AspNetCore.Components;
using Cardmngr.Shared.Utils.Comparer;

namespace Cardmngr.Components.TaskAggregate.Modals;

public partial class TaskDetailsModal() : AddEditModalBase<OnlyofficeTask, TaskUpdateData>(new OnlyofficeTaskUpdateDataEqualityComparer()), 
    IDisposable
{
    private readonly Guid lockGuid = Guid.NewGuid();
    Components.Modals.MyBlazored.Offcanvas currentModal = null!;
    private bool CanEdit => !State.ReadOnly && (Model == null || Model.CanEdit);

    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] ITagColorManager TagColorGetter { get; set; } = null!;
    [Inject] IToastService ToastService { get; set; } = null!;
    [Inject] NotificationHubConnection NotificationHubConnection { get; set; } = null!;

    [Parameter] public IProjectState State { get; set; } = null!;
    [Parameter] public ProjectHubClient? ProjectHubClient { get; set; }
    [Parameter] public int TaskStatusId { get; set; }
    [Parameter] public List<TaskTag> TaskTags { get; set; } = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (State is IRefresheableProjectState refresheableProjectState)
        {
            refresheableProjectState.RefreshService.Lock(lockGuid);
        }

        if (IsAdd)
        {
            buffer.Title = "Новая задача";
            buffer.CustomTaskStatus = TaskStatusId;
        }
        else
        {
            if (ProjectHubClient is { })
            {
                ProjectHubClient.OnUpdatedTask += NotifyThatTaskWasChanged;
                ProjectHubClient.OnDeletedTask += NotifyThatTaskWasDeleted;
            }

            buffer.Status = null; // need because onlyoffice api update by this value on default task's status
        }
        
    }

    private void NotifyThatTaskWasChanged(OnlyofficeTask upd)
    {
        if (upd.Id == Model!.Id)
        {
            ToastService.ShowInfo("Задача была изменена кем-то другим");

            SkipConfirmation = true;
            currentModal.CloseAsync().Forget();
        }
    }

    private void NotifyThatTaskWasDeleted(int taskId)
    {
        if (taskId == Model?.Id)
        {
            ToastService.ShowInfo("Задача была удалена кем-то другим");

            SkipConfirmation = true;
            currentModal.CloseAsync().Forget();
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
            var created = await TaskClient.CreateAsync(State.Project.Id, buffer);
            
            var tagsTasks = TaskTags.Select(x => TaskClient.CreateTagAsync(created.Id, x.Name));
            var tags = await Task.WhenAll(tagsTasks);

            created.Tags = [.. tags];

            State.AddTask(created);

            ProjectHubClient?.SendCreatedTaskAsync(created.ProjectOwner.Id, created.Id).Forget();
            NotificationHubConnection.NotifyAboutCreatedTaskAsync(created).Forget();
        }
        else
        {
            var updated = await TaskClient.UpdateAsync(Model!.Id, buffer);
            State.UpdateTask(updated);

            ProjectHubClient?.SendUpdatedTaskAsync(updated.ProjectOwner.Id, updated.Id).Forget();
        }

        SkipConfirmation = true;
        await currentModal.CloseAsync();
    }

    private async Task DeleteAsync()
    {
        var answer = await ShowDeleteConfirm("Удаление задачи");

        if (answer.Confirmed)
        {
            foreach (var tag in TaskTags ?? [])
                TagColorGetter.RemoveTag(tag);

            await TaskClient.RemoveAsync(Model!.Id);
            State.RemoveTask(Model);
            
            ProjectHubClient?.SendDeletedTaskAsync(Model.ProjectOwner.Id, Model!.Id).Forget();

            SkipConfirmation = true;
            await currentModal.CloseAsync();
        }
    }

    public void Dispose()
    {
        if (State is IRefresheableProjectState refresheableProjectState)
        {
            if (!refresheableProjectState.RefreshService.Disposed)
            {
                refresheableProjectState.RefreshService.RemoveLock(lockGuid);
            }
        }
        
        if (ProjectHubClient is { })
        {
            ProjectHubClient.OnUpdatedTask -= NotifyThatTaskWasChanged;
            ProjectHubClient.OnDeletedTask -= NotifyThatTaskWasDeleted;
        }
    }
}
