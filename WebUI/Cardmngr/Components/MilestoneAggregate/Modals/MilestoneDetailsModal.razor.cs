using Cardmngr.Components.Modals.MyBlazored;
using Cardmngr.Components.ProjectAggregate;
using Microsoft.AspNetCore.Components;
using Cardmngr.Domain.Entities;
using Blazored.Modal;
using Cardmngr.Components.Modals;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Shared.Extensions;
using Onlyoffice.Api.Models;
using Cardmngr.Application.Clients.Milestone;
using Cardmngr.Application.Clients.SignalRHubClients;

namespace Cardmngr.Components.MilestoneAggregate.Modals;

public partial class MilestoneDetailsModal : AddEditModalBase<Milestone, MilestoneUpdateData>, IDisposable
{
    private readonly Guid lockGuid = Guid.NewGuid();
    Offcanvas currentModal = null!;

    private bool CanEdit => Model == null || Model.CanEdit;

    private DateTime? Start => Model == null ? buffer.Deadline?.AddDays(-7) : State.GetMilestoneStart(Model);

    private int TotalTasks => Model == null ? 0 : milestoneTasks.Count();
    private int ActiveTasks => Model == null ? 0 : milestoneTasks.Count(x => !x.IsClosed());

    [Parameter] public ProjectState State { get; set; } = null!;
    [Parameter] public ProjectHubClient ProjectHubClient { get; set; } = null!;

    [Inject] IMilestoneClient MilestoneClient { get; set; } = null!;

    private IEnumerable<OnlyofficeTask> milestoneTasks = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();

        State.RefreshService.Lock(lockGuid);

        if (IsAdd)
        {
            buffer.Title = "Новая веха";
        }
        else
        {
            milestoneTasks = State.GetMilestoneTasks(Model!);
        }
    }

    private async Task SubmitAsync()
    {
        if (IsAdd)
        {
            var added = await MilestoneClient.CreateAsync(State.Model!.Project.Id, buffer);
            State.AddMilestone(added);
        }
        else
        {
            var updated = await MilestoneClient.UpdateAsync(Model!.Id, buffer);
            State.UpdateMilestone(updated);
        }

        await currentModal.CloseAsync();
    }

    private async Task DeleteAsync()
    {
        var answer = await ShowDeleteConfirm("Удаление вехи");

        if (answer.Confirmed)
        {
            await MilestoneClient.RemoveAsync(Model!.Id);
            State.RemoveMilestone(Model!.Id);
            await currentModal.CloseAsync();
        }
    }

    private async Task ShowResponsibleSelectionModal()
    {
        var parameters = new ModalParameters
        {
            { "Items", State.Model!.Team },
            { "ItemRender", UserInfoRenderFragment }
        };

        var answer = await Modal.Show<SelectionModal<UserInfo>>("Выберите ответственного", parameters, MiddleModal).Result;

        if (answer.Confirmed)
        {
            buffer.Responsible = (answer.Data as UserInfo)!.Id;
        }
    }

    public void Dispose() => State.RefreshService.RemoveLock(lockGuid);
}
