using Cardmngr.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Cardmngr.Shared.Helpers
{
    public class SubtaskUpdateEqualityComparer : IEqualityComparer<Subtask>
    {
        public bool Equals(Subtask? x, Subtask? y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                x.Title == y.Title &&
                x.Responsible?.Id == y.Responsible?.Id;
        }

        public int GetHashCode([DisallowNull] Subtask obj)
        {
            return HashCode.Combine(obj.Id, obj.Title, obj.Responsible);
        }
    }
}
