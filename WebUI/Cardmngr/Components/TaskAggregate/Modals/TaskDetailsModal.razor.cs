using Cardmngr.Components.ProjectAggregate;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.Modals.MyBlazored;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.SignalRHubClients;

namespace Cardmngr.Components.TaskAggregate.Modals;

public partial class TaskDetailsModal : AddEditModalBase<OnlyofficeTask, TaskUpdateData>
{
    Offcanvas currentModal = null!;
    private bool CanEdit => Model == null || Model.CanEdit;

    [Inject] public ITaskClient TaskClient { get; set; } = null!;

    [Parameter] public ProjectState State { get; set; } = null!;
    [Parameter] public ProjectHubClient ProjectHubClient { get; set; } = null!;
    [Parameter] public int TaskStatusId { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (IsAdd)
        {
            buffer.Title = "Новая задача";
            buffer.CustomTaskStatus = TaskStatusId;
        }
        else
        {
            ProjectHubClient.OnUpdatedTask += task => 
            {
                if (task.Id == Model!.Id)
                {
                    Model = task;
                    buffer = Mapper.Map<TaskUpdateData>(task);
                    StateHasChanged();
                }
            };

            ProjectHubClient.OnDeletedTask += async taskId =>
            {
                if (taskId == Model?.Id)
                {
                    await currentModal.CloseAsync();
                }
            };

            buffer.Status = null; // need because onlyoffice api update by this value on default task's status
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
}
