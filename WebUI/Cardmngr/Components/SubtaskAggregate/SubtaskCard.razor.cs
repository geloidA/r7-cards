using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Cardmngr.Application.Clients.Subtask;
using Microsoft.FluentUI.AspNetCore.Components;
using KolBlazor;
using Cardmngr.Components.TaskAggregate.ModalComponents;
using Cardmngr.Utils;
using Onlyoffice.Api.Models;

namespace Cardmngr.Components.SubtaskAggregate;

public partial class SubtaskCard : KolComponentBase, IDisposable
{
    private SubtaskUpdateData _buffer = null!;
    private SubtaskInteraction SubtaskInteraction => new(Subtask.Id, InteractionMode);

    [CascadingParameter] TaskSubtasks Parent { get; set; } = null!;
    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] OnlyofficeTask Task { get; set; } = null!;

    [Inject] private ISubtaskClient SubtaskClient { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;

    [Parameter, EditorRequired]
    public Subtask Subtask { get; set; } = null!;

    [Parameter]
    public EventCallback<SubtaskInteraction> OnSubtaskInteractionChange { get; set; }

    [Parameter]
    public InteractionMode InteractionMode { get; set; }

    private bool Disabled => Task.IsClosed() || !Subtask.CanEdit || State.ReadOnly;

    private string CssDisabled => Disabled ? "" : "cursor-pointer hover:bg-neutral-fill-hover transition-colors";
    private string CssCompleted => Subtask.Status == Status.Closed ? "text-info line-through" : "";

    protected override void OnInitialized()
    {
        _buffer = new SubtaskUpdateData
        {
            Title = Subtask.Title,
            Status = (int)Subtask.Status,
            Responsible = Subtask.Responsible?.Id
        };

        Parent.NotifySubtaskStopEditing += NotifySubtaskStopEditing;
    }

    private void NotifySubtaskStopEditing(int subtaskId)
    {
        if (subtaskId == Subtask.Id && InteractionMode == InteractionMode.Edit)
        {
            // Doesn't notify parent that subtask stopped editing, because parent is actually know that
            InteractionMode = InteractionMode.None;
            PreventBufferChanges();
        }
    }

    private async Task TurnOnEditMode()
    {
        if (Disabled || InteractionMode == InteractionMode.Edit) return;
        InteractionMode = InteractionMode.Edit;
        await OnSubtaskInteractionChange.InvokeAsync(SubtaskInteraction);
    }

    private async Task EndEditMode()
    {
        InteractionMode = InteractionMode.None;
        await OnSubtaskInteractionChange.InvokeAsync(SubtaskInteraction);
    }

    private async Task ChangeSubtaskStatus(bool isCompleted)
    {
        try
        {
            var updated = await SubtaskClient.UpdateSubtaskStatusAsync(Task.Id, Subtask.Id, isCompleted ? Status.Closed : Status.Open);
            State.UpdateSubtask(updated);
        }
        catch (HttpRequestException e)
        {
            ToastService.ShowError(e.Message);
        }

        if (InteractionMode == InteractionMode.Edit)
        {
            PreventBufferChanges();
            await EndEditMode();
        }
    }

    private async Task DeleteSubtask()
    {
        try
        {
            await SubtaskClient.RemoveAsync(Task.Id, Subtask.Id);
            State.RemoveSubtask(Task.Id, Subtask.Id);
        }
        catch (HttpRequestException e)
        {
            ToastService.ShowError(e.Message);
        }
    }

    private async Task ChangeSubtask()
    {
        try
        {
            if (InteractionMode == InteractionMode.Edit)
            {
                var updated = await SubtaskClient.UpdateAsync(Task.Id, Subtask.Id, _buffer);
                State.UpdateSubtask(updated);
            }
            else if (InteractionMode == InteractionMode.Add)
            {
                var created = await SubtaskClient.CreateAsync(Task.Id, _buffer);
                State.AddSubtask(Task.Id, created);
            }
        }
        catch (HttpRequestException e)
        {
            ToastService.ShowError(e.Message);
            return;
        }

        await EndEditMode();
    }

    public IEnumerable<UserInfo?> SelectedResponsible
    {
        get => _buffer.Responsible is not null
            ? [State.Team.First(x => x.Id == _buffer.Responsible)] 
            : [];
        set => _buffer.Responsible = value.FirstOrDefault()?.Id;
    }

    private void OnSearchResponsible(OptionsSearchEventArgs<UserInfo> e)
    {
        e.Items = State.Team.Where(x => x.DisplayName.StartsWith(e.Text, StringComparison.OrdinalIgnoreCase));
    }

    private Task Cancel()
    {        
        PreventBufferChanges();
        return EndEditMode();
    }

    private void PreventBufferChanges()
    {
        _buffer.Title = Subtask.Title;
        _buffer.Responsible = Subtask.Responsible?.Id;
    }

    public void Dispose()
    {
        Parent.NotifySubtaskStopEditing -= NotifySubtaskStopEditing;
    }
}
