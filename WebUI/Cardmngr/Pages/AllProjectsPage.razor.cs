using Cardmngr.Application.Clients;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Pages;

[Authorize]
public partial class AllProjectsPage : AuthorizedPage, IDisposable
{
    private bool initialized;
    private List<StaticProjectVm> allProjects = [];

    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] IProjectFollowChecker ProjectFollowChecker { get; set; } = null!;
    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        SummaryService.FilterManager.FilterChanged += OnFilterChangedAsync;
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

    private async void OnFilterChangedAsync(TaskFilterBuilder builder)
    {
        allProjects = await ProjectClient.GetGroupedFilteredTasksAsync(builder.SortBy("updated").SortOrder(FilterSortOrders.Desc))
            .Select(x => new StaticProjectVm(x.Key, x.Value))
            .OrderByDescending(x => ProjectFollowChecker.IsFollow(x.ProjectInfo.Id))
            .ToListAsync();

        SummaryService.SetTasks(allProjects.SelectMany(x => x.Tasks).ToList());
        
        StateHasChanged();
    }

    public void Dispose()
    {
        SummaryService.LeftPage();
        SummaryService.FilterManager.FilterChanged -= OnFilterChangedAsync;
    }
}
