using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Shared.Utils.Comparer;

public class OnlyofficeTaskUpdateDataEqualityComparer : IEqualityComparer<OnlyofficeTask, TaskUpdateData>
{
    public bool Equals(OnlyofficeTask? x, TaskUpdateData? y)
    {
        if (y == null) return false;

        if (x == null)
        {
            return y.Title == "Новая задача" &&
                   string.IsNullOrEmpty(y.Description) &&
                   y.Deadline == null &&
                   y.Priority == 0 &&
                   y.MilestoneId == null &&
                   y.StartDate == null &&
                   y.Responsibles.Count == 0;
        }

        return x.Title == y.Title &&
               x.Description == y.Description &&
               x.Deadline == y.Deadline &&
               (int)x.Priority == y.Priority &&
               x.Milestone?.Id == y.MilestoneId &&
               x.StartDate == y.StartDate &&
               x.Responsibles.Select(r => r.Id).SequenceEqual(y.Responsibles);
    }

    public int GetHashCode(OnlyofficeTask? obj)
    {
        if (obj == null) return 0;
        return obj.GetHashCode();
    }

    public int GetHashCode(TaskUpdateData? obj)
    {
        if (obj == null) return 0;
        return obj.GetHashCode();
    }
}
