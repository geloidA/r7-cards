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
    [CascadingParameter] ProjectHubClient ProjectHubClient { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")] ModalOptions Options { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;
    [CascadingParameter] HeaderInteractionService ProjectInfo { get; set; } = null!;

    protected override void OnInitialized()
    {
        ProjectInfo.OpenProjectInfoFunc = ShowProjectMenu;
        ProjectInfo.ProjectTitle = State.Project.Title;
    }

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
