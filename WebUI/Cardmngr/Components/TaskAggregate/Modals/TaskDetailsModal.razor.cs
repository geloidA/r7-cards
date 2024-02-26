using Cardmngr.Components.ProjectAggregate;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.Modals.MyBlazored;

namespace Cardmngr.Components.TaskAggregate.Modals;

public partial class TaskDetailsModal
{
    Offcanvas currentModal = null!;
    private bool CanEdit => Model == null || Model.CanEdit;

    [Parameter] public ProjectState State { get; set; } = null!;
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
            buffer.Status = null; // need because onlyoffice api update by this value on default task's status
        }
        
    }

    private async Task SubmitAsync()
    {
        if (IsAdd)
        {
            await State.AddTaskAsync(buffer);
        }
        else
        {
            await State.UpdateTaskAsync(Model!.Id, buffer);
        }

        await currentModal.CloseAsync();
    }

    private async Task DeleteAsync()
    {
        var answer = await ShowDeleteConfirm("Удаление задачи");

        if (answer.Confirmed)
        {
            await State.RemoveTaskAsync(Model!.Id);
            await currentModal.CloseAsync();
        }
    }
}
