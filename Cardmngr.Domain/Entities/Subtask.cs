using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public class Subtask : AuditableEntityBase<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int TaskId { get; set; }
        public bool CanEdit { get; set; }
        public UserInfo Responsible { get; set; }
        public Status Status { get; set; }
    }
}
