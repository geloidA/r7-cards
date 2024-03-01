using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.ProjectAggregate.Modals;
using Cardmngr.Application.Clients.SignalRHubClients;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoardHeader : ComponentBase
{
    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] ProjectHubClient ProjectHubClient { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")] ModalOptions Options { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    private async Task ShowProjectMenu()
    {
        var parameters = new ModalParameters 
        { 
            { "State", State },
            { "ProjectHubClient", ProjectHubClient }
        };
        await Modal.Show<ProjectDetailsModal>("",  parameters, Options).Result;
    }
}
