﻿using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Shared.Utils.Comparer;

public class MilestoneMilestoneUpdateDataEqualityComparer : IEqualityComparer<Milestone, MilestoneUpdateData>
{
    public bool Equals(Milestone? x, MilestoneUpdateData? y)
    {
        if (x == null || y == null) return false;
        return x.Description == y.Description &&
            x.Title == y.Title &&
            x.IsKey == y.IsKey &&
            x.Deadline == y.Deadline &&
            x.Responsible.Id == y.Responsible;
    }

    public int GetHashCode(Milestone? obj)
    {
        if (obj == null) return 0;
        return obj.GetHashCode();
    }

    public int GetHashCode(MilestoneUpdateData? obj)
    {
        if (obj == null) return 0;
        return obj.GetHashCode();
    }
}