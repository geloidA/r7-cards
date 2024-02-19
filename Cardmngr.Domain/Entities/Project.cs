using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public record class Project : AuditableEntityBase<int>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public ProjectStatus Status { get; init; }
        public UserInfo Responsible { get; init; }
        public bool CanEdit { get; init; }
        public bool CanDelete { get; init; }
        public bool IsPrivate { get; init; }
    }
}
