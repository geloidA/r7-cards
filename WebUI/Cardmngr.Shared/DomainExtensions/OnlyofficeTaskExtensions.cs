using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Extensions;

namespace Cardmngr.Shared.Extensions;

public static class OnlyofficeTaskExtensions
{
    public static bool IsClosed(this IOnlyofficeTask task)
    {
        return task.Status == Status.Closed;
    }

    public static bool IsDeadlineOut(this IOnlyofficeTask task, DateTime? deadline = null)
    {
        return !task.IsClosed() && DateTime.Now.Date > (deadline?.Date ?? task.Deadline?.Date);
    }

    public static bool IsDeadlineOut(this IOnlyofficeTask task, Milestone milestone)
    {
        if (task.MilestoneId != milestone.Id)
            throw new ArgumentException("Task and milestone must be in the same milestone");

        return task.IsDeadlineOut(milestone.Deadline);
    }

    public static bool IsSevenDaysDeadlineOut(this IOnlyofficeTask task, DateTime? deadline = null)
    {
        return !task.IsClosed() && DateTime.Now.AddDays(6) > (deadline ?? task.Deadline);
    }

    public static IEnumerable<OnlyofficeTask> FilterByStatus(this IEnumerable<OnlyofficeTask> tasks, OnlyofficeTaskStatus status)
    {
        return tasks.Where(x => x.HasStatus(status));
    }

    public static bool HasStatus(this IOnlyofficeTask task, OnlyofficeTaskStatus status)
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
            .OrderByDescending(x => (x.IsDeadlineOut(), x.Priority, x.Updated))
            .ThenBy(x => (x.Deadline == null, x.Deadline));
    }

    public static IEnumerable<OnlyofficeTask> OrderByShortTaskCriteria(this IEnumerable<OnlyofficeTask> tasks)
    {
        return tasks
            .OrderByDescending(x => (x.IsDeadlineOut(), x.Priority, -(int)x.Status))
            .ThenBy(x => (x.Deadline == null, x.Deadline));
    }
}
