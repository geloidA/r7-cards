using Cardmngr.Components.Modals.MyBlazored;
using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.AspNetCore.Components;
using Cardmngr.Domain.Entities;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Shared.Extensions;
using Onlyoffice.Api.Models;
using Cardmngr.Application.Clients.Milestone;
using Cardmngr.Extensions;
using Cardmngr.Shared.Utils.Comparer;
using Blazored.Modal.Services;
using Cardmngr.Utils;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.MilestoneAggregate.Modals;

public sealed partial class MilestoneDetailsModal() : 
    AddEditModalBase<Milestone, MilestoneUpdateData>(new MilestoneMilestoneUpdateDataEqualityComparer()), 
    IDisposable
{
    private Guid lockGuid;
    private Offcanvas currentModal = null!;
    private IEnumerable<OnlyofficeTask> milestoneTasks = [];

    private bool CanEdit => !State.ReadOnly && (Model == null || Model.CanEdit);

    private DateTime? Start => Model == null ? Buffer.Deadline?.AddDays(-7) : State.GetMilestoneStart(Model, Buffer.Deadline);

    [Inject] private IMilestoneClient MilestoneClient { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    [Parameter] public IProjectState State { get; set; } = null!;

    public void Dispose()
    {
        if (State is IRefreshableProjectState refreshableState)
        {
            refreshableState.RefreshService.RemoveLock(lockGuid);
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (State is IRefreshableProjectState refreshableState)
        {
            lockGuid = Guid.NewGuid();
            refreshableState.RefreshService.Lock(lockGuid);
        }

        if (IsAdd)
        {
            Buffer.Title = "Новая веха";
        }
        else
        {
            milestoneTasks = State.GetMilestoneTasks(Model!);
        }
    }

    private async Task ToggleMilestoneStatus()
    {
        Milestone updated;

        try
        {
            updated = await MilestoneClient.UpdateStatusAsync(Model!.Id, Model!.IsClosed() ? 0 : 1);
        }
        catch (HttpRequestException ex)
        {
            ToastService.ShowError(ex.Message);
            return;
        }

        State.UpdateMilestone(updated);
        Model = updated;
    }

    private async Task SubmitAsync()
    {
        if (enterPressed)
        {
            enterPressed = false;
            return;
        }

        if (IsAdd)
        {
            Milestone added;

            try
            {
                added = await MilestoneClient.CreateAsync(State.Project.Id, Buffer);
            }
            catch (HttpRequestException ex)
            {
                ToastService.ShowError(ex.Message);
                return;
            }

            State.AddMilestone(added);
        }
        else
        {
            Milestone updated;

            try
            {
                updated = await MilestoneClient.UpdateAsync(Model!.Id, Buffer);
            }
            catch (HttpRequestException ex)
            {
                ToastService.ShowError(ex.Message);
                return;
            }

            State.UpdateMilestone(updated);
        }

        SkipConfirmation = true;
        await currentModal.CloseAsync(ModalResult.Ok(IsAdd ? ModalResultType.Added : ModalResultType.Edited));
    }

    private async Task DeleteAsync()
    {
        var answer = await ShowDeleteConfirm("Удаление вехи");

        if (answer.Confirmed)
        {
            try
            {
                await MilestoneClient.RemoveAsync(Model!.Id);
            }
            catch (HttpRequestException ex)
            {
                ToastService.ShowError(ex.Message);
                return;
            }
            
            State.RemoveMilestone(Model);
            
            SkipConfirmation = true;
            await currentModal.CloseAsync(ModalResult.Ok(ModalResultType.Deleted));
        }
    }
}
