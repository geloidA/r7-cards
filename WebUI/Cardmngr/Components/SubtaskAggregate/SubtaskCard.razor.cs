using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Cardmngr.Application.Clients.Subtask;
using Microsoft.FluentUI.AspNetCore.Components;
using Blazored.Modal.Services;
using Blazored.Modal;
using Cardmngr.Components.Modals.ConfirmModals;

namespace Cardmngr.Components.SubtaskAggregate;

public partial class SubtaskCard : ComponentBase
{
    private string proxyUrl = null!;

    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] OnlyofficeTask Task { get; set; } = null!;

    // DefaultConfirmModal

    [Inject] IConfiguration Config { get; set; } = null!;
    [Inject] public ISubtaskClient SubtaskClient { get; set; } = null!;

    [CascadingParameter(Name = "MiddleModal")] ModalOptions ModalOptions { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = default!;

    [Parameter, EditorRequired]
    public Subtask Subtask { get; set; } = null!;

    protected override void OnInitialized()
    {
        proxyUrl = Config.CheckKey("proxy-url");
    }

    private string CssCompleted => Subtask.Status == Status.Closed ? "completed" : "";
    private Icon StatusIcon => Subtask.Status == Status.Open 
        ? new Icons.Regular.Size20.CheckmarkSquare() 
        : new Icons.Filled.Size20.CheckmarkSquare();

    private async Task ChangeSubtaskStatus(Status status)
    {
        var updated = await SubtaskClient.UpdateSubtaskStatusAsync(Task.Id, Subtask.Id, status);
        State.UpdateSubtask(updated);
    }

    private async Task DeleteSubtask()
    {
        var confirmModal = await Modal.Show<DefaultConfirmModal>("Удаление подзадачи", ModalOptions).Result;
        if (confirmModal.Cancelled) return;
        
        await SubtaskClient.RemoveAsync(Task.Id, Subtask.Id);
        State.RemoveSubtask(Task.Id, Subtask.Id);
    }
}
