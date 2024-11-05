using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Application.Clients.Project;
using Cardmngr.Components.MilestoneAggregate.Modals;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Components.TaskAggregate.Modals;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Modals;

public sealed partial class ProjectDetailsModal : ComponentBase, IDisposable
{
    private bool _canShowCreateTaskModal = true;
    private bool _canShowMilestoneModal = true;
    private readonly Guid lockGuid = Guid.NewGuid();

    [Inject] private IProjectClient ProjectClient { get; set; } = null!;
    
    [Parameter] public IProjectState State { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")]
    private ModalOptions Options { get; set; } = null!;
    
    [CascadingParameter] 
    private IModalService Modal { get; set; } = null!;

    private Icon IconFollow => State.Project.IsFollow ? new Icons.Filled.Size20.Star() : new Icons.Regular.Size20.Star();

    protected override void OnInitialized()
    {
        if (State is IRefreshableProjectState refreshableProjectState)
        {
            refreshableProjectState.RefreshService.Lock(lockGuid);
        }
    }

    public async Task ToggleFollowAsync()
    {
        await ProjectClient.FollowProjectAsync(State.Project.Id).ConfigureAwait(false);
        State.Project.IsFollow = !State.Project.IsFollow;
    }

    private async Task ShowMilestoneCreation()
    {
        if (!_canShowMilestoneModal) return;

        _canShowMilestoneModal = false;

        var parameters = new ModalParameters
        {
            { "IsAdd", true },
            { "State", State }
        };

        await Modal.Show<MilestoneDetailsModal>(parameters, Options).Result;

        _canShowMilestoneModal = true;
    }

    private async Task ShowCreateTaskModal()
    {
        if (!_canShowCreateTaskModal) return;

        _canShowCreateTaskModal = false;
        var defaultOpen = State.Statuses.First(x => x.IsDefault && x.StatusType == StatusType.Open);

        var parameters = new ModalParameters
        {
            { "State", State },
            { "IsAdd", true },
            { "TaskStatusId", defaultOpen.Id }
        };

        await Modal.Show<TaskDetailsModal>(parameters, Options).Result;

        _canShowCreateTaskModal = true;
    }

    public void Dispose()
    {
        if (State is IRefreshableProjectState refreshableProjectState)
        {
            refreshableProjectState.RefreshService.RemoveLock(lockGuid);
        }
    }
}
