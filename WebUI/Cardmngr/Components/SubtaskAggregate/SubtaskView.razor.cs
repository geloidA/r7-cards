using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models;

namespace Cardmngr.Components.SubtaskAggregate;

public partial class SubtaskView
{    
    private bool isEditMode;

    [CascadingParameter] ProjectState State { get; set; } = null!;
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
        
        await State.UpdateSubtaskStatusAsync(Task.Id, Id, (Status)Subtask.Status);
    }

    private async Task DeleteSubtask()
    {
        await State.RemoveSubtaskAsync(Task.Id, Id);
        await UpdateCallback.InvokeAsync();
    }

    private async Task Submit()
    {
        await State.UpdateSubtaskAsync(Task.Id, Id, Subtask);
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
