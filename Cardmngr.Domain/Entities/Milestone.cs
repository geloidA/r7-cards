using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public record class Milestone : AuditableEntityBase<int>
    {
        public bool CanEdit { get; init; }
        public bool CanDelete { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public Project Project { get; init; }
        public DateTime Deadline { get; init; }
        public bool IsKey { get; init; }
        public bool IsNotify { get; init; }
        public Status Status { get; init; }
        public UserInfo Responsible { get; init; }
    }
}
