using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public class Milestone : AuditableEntityBase<int>
    {
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsKey { get; set; }
        public bool IsNotify { get; set; }
        public Status Status { get; set; }
        public UserInfo Responsible { get; set; }
    }
}
