using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Shared.Utils.Comparer;

public class OnlyofficeTaskUpdateDataEqualityComparer : IEqualityComparer<OnlyofficeTask, TaskUpdateData>
{
    public bool Equals(OnlyofficeTask? x, TaskUpdateData? y)
    {
        if (x == null || y == null) return false;
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
