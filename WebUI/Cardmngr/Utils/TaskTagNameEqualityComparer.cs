using System.Diagnostics.CodeAnalysis;
using Cardmngr.Domain.Entities;

namespace Cardmngr.Utils;

public class TaskTagNameEqualityComparer : IEqualityComparer<TaskTag>
{
    public bool Equals(TaskTag? x, TaskTag? y)
    {
        if (x == null || y == null) return false;
        return x?.Name == y?.Name;
    }

    public int GetHashCode([DisallowNull] TaskTag obj)
    {
        return obj.Name.GetHashCode();
    }
}
