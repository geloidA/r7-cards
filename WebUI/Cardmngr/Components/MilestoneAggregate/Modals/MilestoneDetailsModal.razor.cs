using Cardmngr.Components.Modals.MyBlazored;
using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.AspNetCore.Components;
using Cardmngr.Domain.Entities;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Shared.Extensions;
using Onlyoffice.Api.Models;
using Cardmngr.Application.Clients.Milestone;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using Cardmngr.Shared.Utils.Comparer;

namespace Cardmngr.Components.MilestoneAggregate.Modals;

public sealed partial class MilestoneDetailsModal() : 
    AddEditModalBase<Milestone, MilestoneUpdateData>(new MilestoneMilestoneUpdateDataEqualityComparer()), 
    IDisposable
{
    private Guid lockGuid;
    private Offcanvas currentModal = null!;
    private IEnumerable<OnlyofficeTask> milestoneTasks = [];

    private bool CanEdit => !State.ReadOnly && (Model == null || Model.CanEdit);

    private DateTime? Start => Model == null ? Buffer.Deadline?.AddDays(-7) : State.GetMilestoneStart(Model);

    private int TotalTasks => Model == null ? 0 : milestoneTasks.Count();
    private int ActiveTasks => Model == null ? 0 : milestoneTasks.Count(x => !x.IsClosed());

    [Inject] private IMilestoneClient MilestoneClient { get; set; } = null!;
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
        var updated = await MilestoneClient.UpdateStatusAsync(Model!.Id, Model!.IsClosed() ? 0 : 1);

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
            var added = await MilestoneClient.CreateAsync(State.Project.Id, Buffer);
            State.AddMilestone(added);
        }
        else
        {
            var updated = await MilestoneClient.UpdateAsync(Model!.Id, Buffer);
            State.UpdateMilestone(updated);
        }

        SkipConfirmation = true;
        await currentModal.CloseAsync();
    }

    private async Task DeleteAsync()
    {
        var answer = await ShowDeleteConfirm("Удаление вехи");

        if (answer.Confirmed)
        {
            await MilestoneClient.RemoveAsync(Model!.Id);
            State.RemoveMilestone(Model);
            
            SkipConfirmation = true;
            await currentModal.CloseAsync();
        }
    }

    private IEnumerable<UserInfo> SelectedResponsible
    {
        get => State.Team.FirstOrDefault(x => x.Id == Buffer.Responsible) is { } 
            ? [State.Team.Single(x => x.Id == Buffer.Responsible)] 
            : [];
        set => Buffer.Responsible = value.FirstOrDefault()?.Id;
    }

    private void OnSearchResponsible(OptionsSearchEventArgs<UserInfo> e)
    {
        e.Items = State.Team.Where(x => x.DisplayName.StartsWith(e.Text, StringComparison.OrdinalIgnoreCase));
    }
}
