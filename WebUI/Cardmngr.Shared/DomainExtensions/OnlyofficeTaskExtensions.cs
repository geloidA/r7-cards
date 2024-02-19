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
        return DateTime.Now > task.Deadline;
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

    public static void CloseAllSubtasks(this OnlyofficeTask task)
    {
        foreach (var subtask in task.Subtasks)
        {
            subtask.Status = Status.Closed;
        }
    }
}
