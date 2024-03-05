using Cardmngr.Application.Clients;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Pages;

[Authorize]
public partial class SelfTasksPage : AuthorizedPage
{
    private readonly List<ProjectStateVm> projectVms = [];

    [Inject] IProjectClient ProjectClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await foreach (var projectVm in ProjectClient.GetProjectsWithSelfTasksAsync())
        {
            projectVms.Add(projectVm);
        }
    }

    internal void RemoveProject(int? projectId)
    {
        if (projectVms.RemoveAll(x => x.Project?.Id == projectId) > 0)
        {
            StateHasChanged();
        }
    }
}
