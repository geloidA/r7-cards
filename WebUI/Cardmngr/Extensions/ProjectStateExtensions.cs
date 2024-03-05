using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Extensions;

namespace Cardmngr.Extensions;

public static class ProjectStateExtensions
{
    public static IEnumerable<OnlyofficeTask> GetMilestoneTasks(this IProjectState projectState, Milestone milestone)
    {
        return projectState.Model?.Tasks.Where(x => x.MilestoneId == milestone.Id) ?? [];
    }

    public static IEnumerable<OnlyofficeTask> GetMilestoneTasks(this IProjectState projectState, int milestoneId)
    {
        return projectState.Model?.Tasks.Where(x => x.MilestoneId == milestoneId) ?? [];
    }

    public static IEnumerable<OnlyofficeTask>? SelectedMilestoneTasks(this IProjectState projectState)
    {
        return projectState.SelectedMilestones.Any()
            ? projectState.Model?.Tasks.FilterByMilestones(projectState.SelectedMilestones)
            : projectState.Model?.Tasks;
    }

    public static DateTime? Start(this IProjectState projectState)
    {
        return projectState.Model?.Tasks.Min(x => x.StartDate);
    }

    public static DateTime? Deadline(this IProjectState projectState)
    {
        return projectState.Model?.Tasks.Max(x => x.Deadline);
    }

    public static DateTime GetMilestoneStart(this IProjectState projectState, Milestone milestone)
    {
        var minStart = projectState.Model?.Tasks
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
        return projectState.Model?.Milestones.FirstOrDefault(x => x.Id == milestoneId);
    }

    public static int CountOpenTasks(this IProjectState projectState)
    {
        return projectState.Model?.Tasks.Count(x => x.Status == Domain.Enums.Status.Open) ?? 0;
    }

    public static int CountClosedTasks(this IProjectState projectState)
    {
        return projectState.Model?.Tasks.Count(x => x.Status == Domain.Enums.Status.Closed) ?? 0;
    }
}
