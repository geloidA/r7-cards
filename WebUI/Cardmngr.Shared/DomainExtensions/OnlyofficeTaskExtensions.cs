using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Extensions;

namespace Cardmngr.Shared.Extensions;

public static class OnlyofficeTaskExtensions
{
    public static bool IsClosed(this OnlyofficeTask task)
    {
        return task.Status == Status.Closed;
    }

    public static bool IsDeadlineOut(this OnlyofficeTask task)
    {
        return !task.IsClosed() && DateTime.Now > task.Deadline;
    }

    public static IEnumerable<OnlyofficeTask> FilterByStatus(this IEnumerable<OnlyofficeTask> tasks, OnlyofficeTaskStatus status)
    {
        return tasks.Where(x => x.HasStatus(status));
    }

    public static IEnumerable<OnlyofficeTask> FilterByMilestones(this IEnumerable<OnlyofficeTask> tasks, IEnumerable<Milestone> milestones)
    {
        return tasks.Where(x => milestones.Any(m => m.Id == x.MilestoneId));
    }

    public static bool HasStatus(this OnlyofficeTask task, OnlyofficeTaskStatus status)
    {
        return task.TaskStatusId is null
            ? status.IsDefault && task.Status.ToStatusType() == status.StatusType
            : task.TaskStatusId == status.Id;
    }

    public static bool HasUnclosedSubtask(this OnlyofficeTask task)
    {
        return task.Subtasks.Any(x => x.Status == Status.Open);
    }

    public static IEnumerable<OnlyofficeTask> OrderByTaskCriteria(this IEnumerable<OnlyofficeTask> tasks)
    {
        return tasks
            .OrderByDescending(x => (x.IsDeadlineOut(), -(int)x.Status, x.Priority))
            .ThenBy(x => (x.Deadline is { }, x.Deadline));
    }
}
