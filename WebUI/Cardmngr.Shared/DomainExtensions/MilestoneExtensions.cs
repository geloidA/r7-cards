using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Extensions;

public static class MilestoneExtensions
{
    public static bool IsClosed(this Milestone milestone)
    {
        return milestone.Status == Domain.Enums.Status.Closed;
    }

    public static bool IsDeadlineOut(this Milestone task)
    {
        return DateTime.Now > task.Deadline;
    }
}
