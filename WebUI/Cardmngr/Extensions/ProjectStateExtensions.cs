using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Entities.Base;
using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Extensions;

public static class ProjectStateExtensions
{
    public static IEnumerable<OnlyofficeTask> GetMilestoneTasks(this IProjectStateViewer projectState, Milestone milestone)
    {
        return projectState.Tasks.Where(x => x.MilestoneId == milestone.Id);
    }

    public static IEnumerable<OnlyofficeTask> GetMilestoneTasks(this IProjectStateViewer projectState, int milestoneId)
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

    public static DateTime? Start(this IProjectStateViewer projectState)
    {
        var milestoneStartDates = projectState.Milestones
            .Select(milestone => projectState.GetMilestoneStart(milestone));

        var taskStartDates = projectState.Tasks
            .Select(task => task.StartDate ?? task.Created);

        var minStartDate = milestoneStartDates
            .Union(taskStartDates)
            .DefaultIfEmpty(DateTime.MaxValue)
            .Min();

        return minStartDate == DateTime.MaxValue ? null : minStartDate;
    }


    public static DateTime? Deadline(this IProjectStateViewer projectState)
    {
        var milestoneDeadlineDates = projectState.Milestones.Select(x => (DateTime?)x.Deadline);
        var taskDeadlineDates = projectState.Tasks
            .Select(x => x.GetSmartDeadline())
            .Distinct();

        var maxDeadline = milestoneDeadlineDates
            .Union(taskDeadlineDates)
            .DefaultIfEmpty(DateTime.MinValue)
            .Max();

        return maxDeadline == DateTime.MinValue ? null : maxDeadline;
    }

    public static DateTime GetMilestoneStart(this IProjectStateViewer projectState, Milestone milestone, DateTime? newDeadline = null)
    {
        return GetMilestoneStart(projectState.Tasks, milestone.Id, milestone.Deadline, newDeadline);
    }

    public static DateTime GetMilestoneStart(IEnumerable<OnlyofficeTask> tasks, int milestoneId, DateTime milestoneDeadline, DateTime? newDeadline = null)
    {
        var minStart = tasks
            .Where(x => x.MilestoneId == milestoneId)
            .Min(x => x.StartDate);
        
        var defaultStart = newDeadline?.AddDays(-7) ?? milestoneDeadline.AddDays(-7);
        
        return minStart == null || defaultStart < minStart 
            ? defaultStart : minStart.Value;
    }

    public static Milestone? GetMilestone(this IProjectStateViewer projectState, int? milestoneId)
    {
        if (milestoneId == null) return null;
        return projectState.Milestones.FirstOrDefault(x => x.Id == milestoneId);
    }

    public static int CountOpenTasks(this IProjectStateViewer projectState)
    {
        return projectState.Tasks.Count(x => x.Status == Domain.Enums.Status.Open);
    }

    public static int CountClosedTasks(this IProjectStateViewer projectState)
    {
        return projectState.Tasks.Count(x => x.Status == Domain.Enums.Status.Closed);
    }

    public static int CountClosedTasks(this IFilterableProjectState projectState)
    {
        return projectState.FilteredTasks().Count(x => x.Status == Domain.Enums.Status.Closed);
    }

    public static UserInfo? GetUserById(this IProjectStateViewer projectState, string userId)
    {
        return projectState.Team.FirstOrDefault(x => x.Id == userId);
    }
}
