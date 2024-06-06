
using Cardmngr.Application.Clients;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils.Filter;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public partial class DashboardProjectState : ProjectStateBase, IFilterableProjectState
{
    [Inject] IProjectClient ProjectClient { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }    

    public IFilterManager<OnlyofficeTask> TaskFilter => throw new NotImplementedException();

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await SetModelAsync(await ProjectClient.GetProjectAsync(Id), true);
    }
}
