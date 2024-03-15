using Cardmngr.Components.ProjectAggregate;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Shared.Extensions;
using BlazorBootstrap;
using Cardmngr.Notification;
using Cardmngr.Services;

namespace Cardmngr.Components.TaskAggregate.Modals;

public partial class TaskDetailsModal : AddEditModalBase<OnlyofficeTask, TaskUpdateData>, IDisposable
{
    private readonly Guid lockGuid = Guid.NewGuid();
    Components.Modals.MyBlazored.Offcanvas currentModal = null!;
    private bool CanEdit => Model == null || Model.CanEdit;

    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] TagColorGetter TagColorGetter { get; set; } = null!;
    [Inject] ToastService ToastService { get; set; } = null!;
    [Inject] NotificationHubConnection NotificationHubConnection { get; set; } = null!;

    [Parameter] public IProjectState State { get; set; } = null!;
    [Parameter] public ProjectHubClient? ProjectHubClient { get; set; }
    [Parameter] public int TaskStatusId { get; set; }
    [Parameter] public List<TaskTag>? TaskTags { get; set; }

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
            ToastService.Notify(new ToastMessage 
            { 
                Message = "Задача была изменена кем-то другим",
                Title = "Задача изменена.",
                IconName = IconName.Lamp
            });

            currentModal.CloseAsync().Forget();
        }
    }

    private void NotifyThatTaskWasDeleted(int taskId)
    {
        if (taskId == Model?.Id)
        {
            ToastService.Notify(new ToastMessage 
            { 
                Message = "Задача была удалена",
                Title = "Задача удалена",
                IconName = IconName.EmojiAngry,
                Type = ToastType.Danger
            });

            currentModal.CloseAsync().Forget();
        }
    }

    private async Task SubmitAsync()
    {
        if (IsAdd)
        {
            var created = await TaskClient.CreateAsync(State.Model!.Project!.Id, buffer); // TODO: hide Model in state
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
