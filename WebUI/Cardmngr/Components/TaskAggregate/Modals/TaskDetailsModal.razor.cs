using Cardmngr.Components.ProjectAggregate;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Shared.Extensions;
using BlazorBootstrap;

namespace Cardmngr.Components.TaskAggregate.Modals;

public partial class TaskDetailsModal : AddEditModalBase<OnlyofficeTask, TaskUpdateData>, IDisposable
{
    private readonly Guid lockGuid = Guid.NewGuid();
    Components.Modals.MyBlazored.Offcanvas currentModal = null!;
    private bool CanEdit => Model == null || Model.CanEdit;

    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] ToastService ToastService { get; set; } = null!;

    [Parameter] public ProjectState State { get; set; } = null!;
    [Parameter] public ProjectHubClient ProjectHubClient { get; set; } = null!;
    [Parameter] public int TaskStatusId { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        State.RefreshService.Lock(lockGuid);

        if (IsAdd)
        {
            buffer.Title = "Новая задача";
            buffer.CustomTaskStatus = TaskStatusId;
        }
        else
        {
            ProjectHubClient.OnUpdatedTask += NotifyThatTaskWasChanged;
            ProjectHubClient.OnDeletedTask += NotifyThatTaskWasDeleted;

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
                IconName = IconName.Lamp,

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
            var created = await TaskClient.CreateAsync(State.Model!.Project.Id, buffer);
            State.AddTask(created);
            await ProjectHubClient.SendCreatedTaskAsync(State.Model.Project.Id, created.Id);
        }
        else
        {
            var updated = await TaskClient.UpdateAsync(Model!.Id, buffer);
            State.UpdateTask(updated);
            await ProjectHubClient.SendUpdatedTaskAsync(State.Model!.Project.Id, updated.Id);
        }

        await currentModal.CloseAsync();
    }

    private async Task DeleteAsync()
    {
        var answer = await ShowDeleteConfirm("Удаление задачи");

        if (answer.Confirmed)
        {
            await TaskClient.RemoveAsync(Model!.Id);
            State.RemoveTask(Model!.Id);
            await ProjectHubClient.SendDeletedTaskAsync(State.Model!.Project.Id, Model!.Id);

            await currentModal.CloseAsync();
        }
    }

    public void Dispose()
    {
        State.RefreshService.RemoveLock(lockGuid);
        ProjectHubClient.OnUpdatedTask -= NotifyThatTaskWasChanged;
        ProjectHubClient.OnDeletedTask -= NotifyThatTaskWasDeleted;
    }
}
