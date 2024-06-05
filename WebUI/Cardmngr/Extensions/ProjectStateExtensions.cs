using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Extensions;

public static class ProjectStateExtensions
{
    public static IEnumerable<OnlyofficeTask> GetMilestoneTasks(this IProjectState projectState, Milestone milestone)
    {
        return projectState.Tasks.Where(x => x.MilestoneId == milestone.Id);
    }

    public static IEnumerable<OnlyofficeTask> GetMilestoneTasks(this IProjectState projectState, int milestoneId)
    {
        return projectState.Tasks.Where(x => x.MilestoneId == milestoneId);
    }

    /// <summary>
    /// Filters tasks by projectState's taskFilter 
    /// </summary>
    /// <see cref="IFilterManager{T}"/>
    /// <param name="projectState"></param>
    /// <returns>Filtered tasks</returns>
    public static IEnumerable<OnlyofficeTask> FilteredTasks(this IFilterableProjectState projectState)
    {
        return projectState.Tasks.Filter(projectState.TaskFilter);
    }

    public static DateTime? Start(this IProjectState projectState)
    {
        return projectState.Tasks.Min(x => x.StartDate);
    }

    public static DateTime? Deadline(this IProjectState projectState)
    {
        return projectState.Tasks.Max(x => x.Deadline);
    }

    public static DateTime GetMilestoneStart(this IProjectState projectState, Milestone milestone)
    {
        var minStart = projectState.Tasks
            .Where(x => x.MilestoneId == milestone.Id)
            .Min(x => x.StartDate);
        
        var defaultStart = milestone.Deadline.AddDays(-7);
        
        return minStart == null || defaultStart < minStart 
            ? defaultStart 
            : minStart.Value;
    }

    public static Milestone? GetMilestone(this IProjectState projectState, int? milestoneId)
    {
        if (milestoneId == null) return null;
        return projectState.Milestones.FirstOrDefault(x => x.Id == milestoneId);
    }

    public static int CountOpenTasks(this IProjectState projectState)
    {
        return projectState.Tasks.Count(x => x.Status == Domain.Enums.Status.Open);
    }

    public static int CountClosedTasks(this IProjectState projectState)
    {
        return projectState.Tasks.Count(x => x.Status == Domain.Enums.Status.Closed);
    }
}
