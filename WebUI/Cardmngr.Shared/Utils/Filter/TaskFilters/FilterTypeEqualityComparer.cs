using System.Diagnostics.CodeAnalysis;
using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Shared;

public class FilterTypeEqualityComparer : IEqualityComparer<IFilter>
{
    public bool Equals(IFilter? x, IFilter? y)
    {
        return x?.GetType() == y?.GetType();
    }

    public int GetHashCode([DisallowNull] IFilter obj)
    {
        return obj.GetType().GetHashCode();
    }
}
