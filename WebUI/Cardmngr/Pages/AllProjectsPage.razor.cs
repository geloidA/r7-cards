using Cardmngr.Application.Clients;
using Cardmngr.Components.ProjectAggregate.Vms;
using Cardmngr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Common;

namespace Cardmngr.Pages;

[Authorize]
public partial class AllProjectsPage : AuthorizedPage, IDisposable
{
    private bool initialized;
    private List<StaticProjectVm> allProjects = [];

    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var projs = await ProjectClient.GetProjectsWithTaskFilterAsync(FilterTasksBuilder.Instance.WithMyProjects(true)).ToListAsync();
        allProjects = projs.Select(x => new StaticProjectVm(x)).ToList();
        SummaryService.SetProjects(projs);
        SummaryService.FilterManager.OnFilterChanged += OnFilterChanged;
        SummaryService.FilterManager.Responsible = IdOnInitialization;
        initialized = true;
    }

    private async void OnFilterChanged(FilterTasksBuilder builder)
    {
        builder = SummaryService.FilterManager.ProjectId == null ? builder.WithMyProjects(true) : builder;
        allProjects = await ProjectClient.GetProjectsWithTaskFilterAsync(builder)
            .Select(x => new StaticProjectVm(x))
            .ToListAsync();
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        SummaryService.LeftPage();
    }
}
