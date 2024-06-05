using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Components.MilestoneAggregate.Modals;
using Cardmngr.Components.Modals.MyBlazored;
using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Modals;

public partial class ProjectDetailsModal : ComponentBase, IDisposable
{
    private readonly Guid lockGuid = Guid.NewGuid();
    private Offcanvas currentModal = null!;
    [Parameter] public MutableProjectState State { get; set; } = null!;
    [Parameter] public ProjectHubClient ProjectHubClient { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")] ModalOptions Options { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    Icon IconFollow => State.Project.IsFollow ? new Icons.Filled.Size20.Star() : new Icons.Regular.Size20.Star();

    protected override void OnInitialized()
    {
        State.RefreshService.Lock(lockGuid);
    }

    private void ShowMilestoneCreation()
    {
        var parameters = new ModalParameters
        {
            { "IsAdd", true },
            { "State", State },
            { "ProjectHubClient", ProjectHubClient }
        };

        Modal.Show<MilestoneDetailsModal>("", parameters, Options);
    }

    public void Dispose() => State.RefreshService.RemoveLock(lockGuid);
}
