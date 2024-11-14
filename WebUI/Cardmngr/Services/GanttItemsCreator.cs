using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using KolBlazor.Components.Charts.Data;

namespace Cardmngr.Services;

public class GanttItemsCreator(Dictionary<int, bool> milestoneExpanded)
{
    public Dictionary<int, bool> MilestoneExpanded { get; } = milestoneExpanded;

    public GanttChartItem Create(IProjectState projectState)
    {
        var isExpanded = projectState is not StaticProjectVm statProj || !statProj.IsCollapsed;

        return new GanttChartItem
        {
            Id = GetItemKey(projectState),
            Data = projectState,
            Start = projectState.Start(),
            End = projectState.Deadline(),
            IsExpanded = isExpanded,
            Children = GetChildren(projectState).ToList()
        };
    }

    private IEnumerable<GanttChartItem> GetChildren(IProjectState projectState)
    {
        var milestoneTasks = projectState.Milestones
            .Select(m => Create(m, projectState))
            .Where(x => x.Children.Count > 0);

        var filterableState = projectState as IFilterableProjectState;
        
        var tasks = filterableState?.FilteredTasks() ?? projectState.Tasks;

        return milestoneTasks
            .Concat(tasks
                .Where(t => !t.MilestoneId.HasValue)
                .Select(Create))
            .OrderBy(x => x.Start);
    }


    public GanttChartItem Create(Milestone milestone, IProjectState projectState)
    {
        return new GanttChartItem
        {
            Id = GetItemKey(milestone),
            Data = milestone,
            Start = projectState.GetMilestoneStart(milestone),
            End = milestone.Deadline,
            Children = [.. projectState
                .GetMilestoneTasks(milestone)
                .Select(Create)
                .OrderBy(x => x.Start)],
            IsExpanded = MilestoneExpanded.TryGetValue(milestone.Id, out var value) && value
        };
    }

    public static GanttChartItem Create(OnlyofficeTask task)
    {
        return new GanttChartItem
        {
            Id = GetItemKey(task),
            Data = task,
            CanManipulate = !task.IsClosed() && task.CanEdit,
            Start = task.StartDate ?? task.Created,
            End = task.GetSmartDeadline()
        };    
    }

    public static string GetItemKey(object data)
    {
        return data switch
        {
            OnlyofficeTask task => $"task-{task.Id}",
            Milestone milestone => $"milestone-{milestone.Id}",
            IProjectState state => $"project-{state.Project.Id}",
            _ => throw new NotSupportedException()
        };
    }

    public static (Type, int) GetItemIdByKey(string key)
    {
        var id = int.Parse(key.Split('-')[1]);
        
        return key switch
        {
            var x when x.StartsWith("task-") => (typeof(OnlyofficeTask), id),
            var x when x.StartsWith("milestone-") => (typeof(Milestone), id),
            var x when x.StartsWith("project-") => (typeof(IProjectState), id),
            _ => throw new NotSupportedException()
        };
    }
}
