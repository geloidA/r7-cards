using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Exceptions;
using Cardmngr.Extensions;
using KolBlazor.Components.Charts.Data;
using KolBlazor.Components.Charts.Gantt;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectGantt : ComponentBase
{
    private GanttChart _chart = null!;
    private IList<GanttChartItem> _chartItems = [];

    [CascadingParameter] private IProjectState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.TasksChanged += _ => 
        {
            _chartItems = GetChartItems();
            _chart.RefreshItems();
            StateHasChanged();
        };
    }

    private static string GetItemKey(GanttChartItem item)
    {
        if (item.Data is OnlyofficeTask task)
        {
            return $"task-{task.Id}";
        }
        else if (item.Data is Milestone milestone)
        {
            return $"milestone-{milestone.Id}";
        }
        else if (item.Data is Project project)
        {
            return $"project-{project.Id}";
        }

        throw new NotSupportedException();
    }

    private List<GanttChartItem> GetChartItems()
    {
        var allTasks = State is IFilterableProjectState filterableState
            ? filterableState.FilteredTasks()
            : State.Tasks;

        var milestoneTasks = allTasks
            .Where(t => t.MilestoneId.HasValue)
            .GroupBy(t => t.MilestoneId)
            .Select(g => 
            {
                var milestone = State.GetMilestone(g.Key) ?? throw new NotFoundMilestoneException(g.Key!.Value);
                return new GanttChartItem
                {
                    Data = milestone,
                    Start = State.GetMilestoneStart(milestone),
                    End = milestone.Deadline,
                    Children = State
                        .GetMilestoneTasks(milestone)
                        .Select(x => new GanttChartItem
                        {
                            Data = x,
                            Start = x.StartDate,
                            End = x.Deadline
                        })
                        .ToList()
                };
            });

        var tasks = allTasks
            .Where(t => !t.MilestoneId.HasValue)
            .Select(x => new GanttChartItem
            {
                Data = x,
                Start = x.StartDate,
                End = x.Deadline
            });

        return milestoneTasks
            .Concat(tasks)
            .ToList();
    }
}
