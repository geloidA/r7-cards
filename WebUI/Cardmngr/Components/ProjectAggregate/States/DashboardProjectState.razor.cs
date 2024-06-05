
using Cardmngr.Application.Clients;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public partial class DashboardProjectState : ProjectStateBase
{
    [Inject] IProjectClient ProjectClient { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await SetModelAsync(await ProjectClient.GetProjectAsync(30), true);
    }
}
