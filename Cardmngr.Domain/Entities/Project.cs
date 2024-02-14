using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public class Project : AuditableEntityBase<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public UserInfo Responsible { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool IsPrivate { get; set; }
    }
}
