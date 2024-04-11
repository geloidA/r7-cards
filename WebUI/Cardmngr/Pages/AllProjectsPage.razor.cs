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
    [Inject] ProjectSummaryService ProjectSummaryService { get; set; } = null!;
    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        SummaryService.FilterManager.OnFilterChanged += OnFilterChangedAsync;
        if (SummaryService.FilterManager.Responsible == null)
        {
            SummaryService.FilterManager.Responsible = IdOnInitialization;
        }
        else
        {
            OnFilterChangedAsync(SummaryService.FilterManager.GenerateFilter());
        }
        initialized = true;
    }

    private async void OnFilterChangedAsync(FilterTasksBuilder builder)
    {
        allProjects = await ProjectClient.GetGroupedFilteredTasksAsync(builder)
            .Select(x => new StaticProjectVm(x.Key, x.Value))
            .OrderByDescending(x => ProjectSummaryService.FollowedProjectIds.Contains(x.ProjectInfo.Id))
            .ToListAsync();

        SummaryService.SetTasks(allProjects.SelectMany(x => x.Tasks).ToList());
        
        StateHasChanged();
    }

    public void Dispose()
    {
        SummaryService.LeftPage();
        SummaryService.FilterManager.OnFilterChanged -= OnFilterChangedAsync;
    }
}
