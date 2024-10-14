using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.ProjectAggregate.Modals;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Utils;
using Cardmngr.Components.ProjectAggregate.States;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoardHeader : ComponentBase
{
    private bool collapsed;
    [CascadingParameter] IProjectState State { get; set; } = null!;
}
