using Onlyoffice.Api.Common;

namespace Onlyoffice.Api.Models.Extensions;

public static class TaskExtensions
{
    public static bool CanMarkClosed(this Task task)
    {
        return task.Status == (int)Status.Closed
            || (task.Subtasks?.All(x => x.Status == (int)Status.Closed) ?? false);
    }

    public static bool IsDeadlineOut(this Task task)
    {
        bool hasDeadline = task.Deadline.HasValue;
        bool isPastDeadline = DateTime.Now.Date > task.Deadline;
        bool isNotClosed = task.Status != (int)Status.Closed;

        return hasDeadline && isNotClosed && isPastDeadline;
    }

}
