using Cardmngr.Application.Clients.Project;
using Cardmngr.Application.Extensions;
using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Extensions;
using Cardmngr.Services;
using KolBlazor.Components.Charts.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Pages;

[Authorize]
public partial class AllProjectsPage : ComponentBase, IDisposable
{
    private ProjectGantt projectGantt = null!;
    
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

        OnFilterChangedAsync(SummaryService.FilterManager.GenerateFilter());
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
            projectGantt?.Refresh();

            StateHasChanged();
        });
    }

    private IEnumerable<GanttChartItem> GetGanttChartItems()
    {
        return allProjects
            .Select(p => 
            {
                return new GanttChartItem
                {
                    Data = p.ProjectInfo,
                    Start = p.Tasks.Min(x => x.StartDate),
                    End = p.Tasks.Max(x => x.Deadline),
                    Children = GetGanttProjectMilestones(p),
                };
            })
            .OrderBy(x => x.Start ?? DateTime.MaxValue);
    }

    private static IEnumerable<GanttChartItem> GetGanttProjectMilestones(StaticProjectVm project)
    {
        var milestones = project.Tasks
            .Where(t => t.MilestoneId.HasValue)
            .GroupBy(t => t.MilestoneId)
            .Select(g => 
            {
                var milestone = g.First().Milestone;
                return new GanttChartItem
                {
                    Data = g.First().Milestone,
                    End = milestone.Deadline,
                    Start = ProjectStateExtensions.GetMilestoneStart(g, milestone.Id, milestone.Deadline),
                    Children = g.Select(x => new GanttChartItem
                    {
                        Data = x,
                        Start = x.StartDate,
                        End = x.Deadline
                    })
                    .ToList()
                };
            });

        var tasks = project.Tasks
            .Where(t => !t.MilestoneId.HasValue)
            .Select(x => new GanttChartItem
            {
                Data = x,
                Start = x.StartDate,
                End = x.Deadline
            });

        return milestones
            .Concat(tasks)
            .OrderBy(x => x.Start ?? DateTime.MaxValue);
    }

    public void Dispose()
    {
        SummaryService.LeftPage();
        SummaryService.FilterManager.FilterChanged -= OnFilterChangedAsync;
    }
}
