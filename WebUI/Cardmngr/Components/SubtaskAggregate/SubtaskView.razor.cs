using Cardmngr.Application.Clients.Subtask;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models;

namespace Cardmngr.Components.SubtaskAggregate;

public partial class SubtaskView
{    
    private bool isEditMode;

    [Inject] public ISubtaskClient SubtaskClient { get; set; } = null!;

    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] OnlyofficeTask Task { get; set; } = null!;
    [CascadingParameter] bool HasEditingSubtask { get; set; }

    [Parameter] public SubtaskUpdateData Subtask { get; set; } = null!;
    [Parameter] public EventCallback UpdateCallback { get; set; }
    [Parameter] public int Id { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public EventCallback<bool> EditModeChanged { get; set; }

    private async Task SwitchSubtaskStatus()
    {
        Subtask.Status = Subtask.Status == (int)Status.Open ? (int)Status.Closed : (int)Status.Open;

        var updated = await SubtaskClient.UpdateSubtaskStatusAsync(Task.Id, Id, (Status)Subtask.Status);
        
        State.UpdateSubtask(updated);
    }

    private async Task DeleteSubtask()
    {        
        await SubtaskClient.RemoveAsync(Task.Id, Id);
        State.RemoveSubtask(Task.Id, Id);
        await UpdateCallback.InvokeAsync();
    }

    private async Task Submit()
    {
        if (string.IsNullOrEmpty(Subtask.Title)) return;
         
        var updated = await SubtaskClient.UpdateAsync(Task.Id, Id, Subtask);
        State.UpdateSubtask(updated);
        await UpdateCallback.InvokeAsync();
        await EditModeChanged.InvokeAsync(false);
        isEditMode = false;
    }

    private async Task BlockOthers()
    {
        isEditMode = true;
        await EditModeChanged.InvokeAsync(true);
    }
}
