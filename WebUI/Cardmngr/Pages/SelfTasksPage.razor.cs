using Cardmngr.Application.Clients;
using Cardmngr.Services;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Common;

namespace Cardmngr.Pages;

[Authorize]
public partial class SelfTasksPage : AuthorizedPage
{
    private bool initialized;
    private List<IProjectStateVm> allProjects = [];

    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] FilterManagerService FilterManager { get; set; } = null!;

    public event Action<bool>? CollapseChanged;
    private void ChangeCollapse(bool collapsed) => CollapseChanged?.Invoke(collapsed);

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        allProjects = await ProjectClient.GetProjectsWithTaskFilterAsync(FilterTasksBuilder.Instance).ToListAsync();
        initialized = true;
        FilterManager.OnFilterChanged += OnFilterChanged;
    }

    private async void OnFilterChanged(FilterTasksBuilder builder)
    {
        allProjects = await ProjectClient.GetProjectsWithTaskFilterAsync(builder).ToListAsync();
        await InvokeAsync(StateHasChanged);
    }
}
