using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public sealed record Milestone : WorkEntityBase
    {
        public ProjectInfo ProjectOwner { get; init; }
        public DateTime Deadline { get; init; }
        public bool IsKey { get; init; }
        public bool IsNotify { get; init; }
        public Status Status { get; init; }
        public UserInfo Responsible { get; init; }

        public bool Equals(Milestone other)
        {
            if (other is null) return false;

            return Id == other.Id && 
                Title == other.Title &&
                Description == other.Description &&
                ProjectOwner == other.ProjectOwner &&
                Status == other.Status &&
                Deadline == other.Deadline &&
                IsKey == other.IsKey &&
                Responsible == other.Responsible;
        }
        
        public override int GetHashCode() => Id;

        public override IEnumerable<UserInfo> GetResponsibles()
        {
            return [Responsible];
        }
    }
}
