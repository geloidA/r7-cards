using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.ProjectAggregate.Modals;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoardHeader : ComponentBase
{
    [CascadingParameter] ProjectState State { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")] ModalOptions Options { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    private async Task ShowProjectMenu()
    {
        var parameters = new ModalParameters { { "State", State } };
        await Modal.Show<ProjectDetailsModal>("",  parameters, Options).Result;
    }
}
