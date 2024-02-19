using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Extensions;

public static class OnlyofficeTaskStatusExtensions
{
    /// <summary>
    /// Order statuses by <see cref="OnlyofficeTaskStatus.StatusType"/> and <see cref="OnlyofficeTaskStatus.Order"/> properties
    /// </summary>
    /// <param name="statuses"></param>
    /// <returns></returns>
    public static IEnumerable<OnlyofficeTaskStatus> InLogicalOrder(this IEnumerable<OnlyofficeTaskStatus> statuses)
    {
        return statuses.OrderBy(x => (x.StatusType, x.Order));
    }
}
