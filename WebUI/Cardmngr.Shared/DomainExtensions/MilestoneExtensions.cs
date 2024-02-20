using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Extensions;

public static class MilestoneExtensions
{
    public static bool IsClosed(this Milestone milestone)
    {
        return milestone.Status == Domain.Enums.Status.Closed;
    }

    public static bool IsDeadlineOut(this Milestone milestone)
    {
        return !milestone.IsClosed() && DateTime.Now > milestone.Deadline;
    }

    public static IEnumerable<Milestone> OrderByMilestoneCriteria(this IEnumerable<Milestone> milestones)
    {
        return milestones.OrderBy(x => (x.IsKey, x.Deadline, x.Status));
    }
}
