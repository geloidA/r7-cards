using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Extensions;

namespace Cardmngr.Extensions;

public static class OnlyofficeTaskExtensions
{
    public static string CssDeadline(this OnlyofficeTask task)
    {
        return task.Deadline is null ? "" :
            task.IsDeadlineOut() ? "red-border" :
            task.IsSevenDaysDeadlineOut() ? "warning-border" : "";
    }

    /// <summary>
    /// Returns deadline of the task. If deadline is null and task is closed, then returns Updated date.
    /// </summary>
    /// <param name="task">Onlyoffice task.</param>
    /// <returns>Deadline date or null.</returns>
    public static DateTime? GetSmartDeadline(this OnlyofficeTask task)
    {
        // If deadline is null and task is closed, then return Updated date.
        return task.Deadline == null && task.IsClosed() ? task.Updated : task.Deadline;
    }
}
