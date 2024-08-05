using Cardmngr.Application.Clients;
using Cardmngr.Application.Extensions;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Pages;

[Authorize]
public partial class AllProjectsPage : IDisposable
{
    private bool initialized;
    private List<StaticProjectVm> allProjects = [];
    private string userId = string.Empty;

    [Inject] private IProjectClient ProjectClient { get; set; } = null!;
    [Inject] private IProjectFollowChecker ProjectFollowChecker { get; set; } = null!;
    [Inject] private AllProjectsPageSummaryService SummaryService { get; set; } = null!;

    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        userId = (await AuthenticationState.ConfigureAwait(false)).User.GetNameIdentifier();
        
        SummaryService.FilterManager.FilterChanged += OnFilterChangedAsync;
        if (SummaryService.FilterManager.Responsible == null)
        {
            SummaryService.FilterManager.Responsible = userId;
        }
        else
        {
            OnFilterChangedAsync(SummaryService.FilterManager.GenerateFilter());
        }
        initialized = true;
    }

    private void OnFilterChangedAsync(TaskFilterBuilder builder)
    {
        _ = InvokeAsync(async () =>
        {
            allProjects = await ProjectClient.GetGroupedFilteredTasksAsync(builder.SortBy("updated").SortOrder(FilterSortOrders.Desc))
                .Select(x => new StaticProjectVm(x.Key, x.Value))
                .OrderByDescending(x => ProjectFollowChecker.IsFollow(x.ProjectInfo.Id))
                .ToListAsync().ConfigureAwait(false);

            SummaryService.SetTasks(allProjects.SelectMany(x => x.Tasks).ToList());
            
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        SummaryService.LeftPage();
        SummaryService.FilterManager.FilterChanged -= OnFilterChangedAsync;
    }
}
