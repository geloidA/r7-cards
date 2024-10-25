using BlazorComponentBus;
using Cardmngr.Application.Clients.Project;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.TaskStatusClient;
using Cardmngr.Application.Extensions;
using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Pages.Contracts;
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
    private bool _loading;

    private readonly PageCache _cache = new();
    private ProjectGantt projectGantt = null!;
    private readonly GanttProjectsFinder projectsFinder = new();
    
    private List<StaticProjectVm> allProjects = [];
    private string userId = string.Empty;

    [Inject] private IProjectClient ProjectClient { get; set; } = null!;
    [Inject] private ITaskStatusClient TaskStatusClient { get; set; } = null!;
    [Inject] private ITaskClient TaskClient { get; set; } = null!;
    [Inject] private IProjectFollowChecker ProjectFollowChecker { get; set; } = null!;
    [Inject] private ComponentBus Bus { get; set; } = null!;
    [Inject] private AllProjectsPageSummaryService SummaryService { get; set; } = null!;

    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        userId = (await AuthenticationState.ConfigureAwait(false)).User.GetNameIdentifier();
        SummaryService.FilterManager.FilterChanged += OnFilterChangedAsync;
        OnFilterChangedAsync(SummaryService.FilterManager.GenerateFilter());
        Bus.Subscribe<GanttModeToggled>(OnGanttModeToggled);
    }

    private void OnGanttModeToggled(MessageArgs _)
    {
        StateHasChanged();
        if (SummaryService.GanttModeEnabled)
        {
            projectsFinder.States = allProjects;
            projectGantt?.Refresh();
        }
        else
        {
            projectsFinder.States = [];
        }
    }

    private void OnFilterChangedAsync(TaskFilterBuilder builder)
    {
        InvokeAsync(async () =>
        {
            _loading = true;
            allProjects = await ProjectClient.GetGroupedFilteredTasksAsync(builder.SortBy("updated").SortOrder(FilterSortOrders.Desc))
                .SelectAwait(async x => await CollectProjectStateAsync(x.Key, x.Value))
                .OrderByDescending(x => ProjectFollowChecker.IsFollow(x.Project.Id))
                .ToListAsync().ConfigureAwait(false);
            _loading = false;

            if (allProjects.Count == 1)
            {
                allProjects[0].ToggleCollapsed(TaskClient, SummaryService.GanttModeEnabled);
            }

            SummaryService.SetTasks(allProjects.SelectMany(x => x.Tasks).ToList());

            if (SummaryService.GanttModeEnabled)
            {
                projectsFinder.States = allProjects;
                projectGantt?.Refresh();
            }

            StateHasChanged();
        });
    }

    private async Task<StaticProjectVm> CollectProjectStateAsync(ProjectInfo project, ICollection<OnlyofficeTask> tasks)
    {
        _cache.Statuses ??= await TaskStatusClient
            .GetAllAsync()
            .ToListAsync();

        if (_cache.ProjectCacheById.TryGetValue(project.Id, out var projectCache))
        {
            return new StaticProjectVm(new Shared.Project.ProjectStateDto
            {
                Project = projectCache.Project,
                Statuses = _cache.Statuses,
                Milestones = projectCache.Milestones,
                Team = projectCache.Team,
                Tasks = [.. tasks]
            });
        }

        var state = await ProjectClient
            .CollectProjectWithTasksAsync(tasks, _cache.Statuses)
            .ConfigureAwait(false);

        _cache.ProjectCacheById[project.Id] = new ProjectCache(state.Project, state.Milestones, state.Team);

        return new StaticProjectVm(state);
    }

    private IEnumerable<GanttChartItem> GetGanttChartItems()
    {
        return allProjects
            .Select(p => new GanttChartItem
            {
                Data = p,
                Start = p.Start(),
                End = p.Deadline(),
                Children = GetGanttProjectMilestones(p).ToList(),
                IsExpanded = !p.IsCollapsed
            })
            .OrderBy(x => x.Start ?? DateTime.MaxValue);
    }

    private static IEnumerable<GanttChartItem> GetGanttProjectMilestones(StaticProjectVm state)
    {
        var milestones = state.Milestones
            .Select(milestone => new GanttChartItem
            {
                Data = milestone,
                End = milestone.Deadline,
                Start = state.GetMilestoneStart(milestone),
                Children = state
                    .GetMilestoneTasks(milestone)
                    .Select(x => new GanttChartItem
                    {
                        Data = x,
                        Start = x.StartDate ?? x.Created,
                        End = x.GetSmartDeadline()
                    })
                    .ToList()
            });

        var tasks = state.Tasks
            .Where(t => !t.MilestoneId.HasValue)
            .Select(x => new GanttChartItem
            {
                Data = x,
                Start = x.StartDate ?? x.Created,
                End = x.GetSmartDeadline()
            });

        return milestones
            .Concat(tasks)
            .OrderBy(x => x.Start ?? DateTime.MaxValue);
    }

    public void Dispose()
    {
        SummaryService.LeftPage();
        SummaryService.FilterManager.FilterChanged -= OnFilterChangedAsync;
        Bus.UnSubscribe<GanttModeToggled>(OnGanttModeToggled);
    }

    private record PageCache
    {
        public List<OnlyofficeTaskStatus>? Statuses { get; set; }
        public Dictionary<int, ProjectCache> ProjectCacheById { get; set; } = [];
    }

    private record ProjectCache(
        Project Project,
        List<Milestone> Milestones,
        List<UserProfile> Team);
}
