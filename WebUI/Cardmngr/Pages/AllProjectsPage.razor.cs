using BlazorComponentBus;
using Cardmngr.Application.Clients.Project;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.TaskStatusClient;
using Cardmngr.Application.Extensions;
using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Pages.Contracts;
using Cardmngr.Services;
using Cardmngr.Shared.Utils;
using KolBlazor.Components.Charts.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Pages;

[Authorize]
public partial class AllProjectsPage : ComponentBase, IDisposable
{
    private readonly GanttItemsCreator _ganttItemsCreator = new([]);
    private bool _loading;

    private readonly PageCache _cache = new();
    private ProjectGantt projectGantt = null!;
    private readonly GanttProjectsFinder projectsFinder = new();
    
    private List<StaticProjectVm> allProjects = [];
    private string userId = string.Empty;

    [Inject] private IProjectClient ProjectClient { get; set; } = null!;
    [Inject] private ITaskStatusClient TaskStatusClient { get; set; } = null!;
    [Inject] private ITaskClient TaskClient { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
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

            await Catcher.CatchAsync<HttpRequestException>(async () => 
            {
                allProjects = await ProjectClient.GetGroupedFilteredTasksAsync(builder.SortBy("updated").SortOrder(FilterSortOrders.Desc))
                    .SelectAwait(async x => await CollectProjectStateAsync(x.Key, x.Value))
                    .OrderByDescending(x => ProjectFollowChecker.IsFollow(x.Project.Id))
                    .ToListAsync().ConfigureAwait(false);
            }, 
            ex => ToastService.ShowError(ex.Message));

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

    private void OnGanttItemToggled(GanttChartItem item)
    {
        if (item.Data is StaticProjectVm project)
        {
            project.ToggleCollapsed(TaskClient, true);
        }
        else if (item.Data is Milestone milestone)
        {
            _ganttItemsCreator.MilestoneExpanded[milestone.Id] = item.IsExpanded;
        }
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
