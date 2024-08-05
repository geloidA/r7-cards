using System.Diagnostics.CodeAnalysis;

namespace Cardmngr.Utils.DetailsModal;

public class EqualityModalComparer : IEqualityComparer<IDetailsModal>
{
    public bool Equals(IDetailsModal? x, IDetailsModal? y)
    {
        if (x == null || y == null) return false;
        if (ReferenceEquals(x, y)) return true;

        return x.Guid == y.Guid;
    }

    public int GetHashCode(IDetailsModal obj)
    {
        return obj.Guid.GetHashCode();
    }
}
