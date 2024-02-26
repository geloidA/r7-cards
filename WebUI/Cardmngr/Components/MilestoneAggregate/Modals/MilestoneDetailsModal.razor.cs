using Cardmngr.Components.Modals.MyBlazored;
using Cardmngr.Components.ProjectAggregate;
using Microsoft.AspNetCore.Components;
using Cardmngr.Domain.Entities;
using Blazored.Modal;
using Cardmngr.Components.Modals;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Shared.Extensions;
using Onlyoffice.Api.Models;
using Cardmngr.Utils.DetailsModal;

namespace Cardmngr.Components.MilestoneAggregate.Modals;

public partial class MilestoneDetailsModal : AddEditModalBase<Milestone, MilestoneUpdateData>
{
    Offcanvas currentModal = null!;

    private bool CanEdit => Model == null || Model.CanEdit;

    private DateTime? Start => Model == null ? buffer.Deadline?.AddDays(-7) : State.GetMilestoneStart(Model);

    private int TotalTasks => Model == null ? 0 : milestoneTasks.Count();
    private int ActiveTasks => Model == null ? 0 : milestoneTasks.Count(x => !x.IsClosed());

    [Parameter] public ProjectState State { get; set; } = null!;

    private IEnumerable<OnlyofficeTask> milestoneTasks = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();

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
            await State.AddMilestoneAsync(buffer);
        }
        else
        {
            await State.UpdateMilestoneAsync(Model!.Id, buffer);
        }

        await currentModal.CloseAsync();
    }

    private async Task DeleteAsync()
    {
        var answer = await ShowDeleteConfirm("Удаление вехи");

        if (answer.Confirmed)
        {
            await State.RemoveMilestoneAsync(Model!.Id);
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
}
