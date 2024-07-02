using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public record Subtask : AuditableEntityBase<int>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public int TaskId { get; init; }
        public bool CanEdit { get; init; }
        public UserInfo Responsible { get; init; }
        public Status Status { get; init; }
    }
}
