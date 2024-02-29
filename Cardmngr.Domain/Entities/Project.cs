using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public sealed record class Project : AuditableEntityBase<int>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public ProjectStatus Status { get; init; }
        public UserInfo Responsible { get; init; }
        public bool CanEdit { get; init; }
        public bool CanDelete { get; init; }
        public bool IsPrivate { get; init; }

        public bool Equals(Project other)
        {
            if (other is null) return false;

            return Id == other.Id &&
                Title == other.Title &&
                Description == other.Description &&
                Status == other.Status &&
                Responsible.Id == other.Responsible.Id &&
                IsPrivate == other.IsPrivate;
        }

        public override int GetHashCode() => Id;
    }
}
