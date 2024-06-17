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
}
