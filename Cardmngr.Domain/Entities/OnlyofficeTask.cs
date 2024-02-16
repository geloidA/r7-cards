using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public class OnlyofficeTask : AuditableEntityBase<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public bool CanEdit { get; set; }
        public bool CanCreateSubtask { get; set; }
        public bool CanCreateTimeSpend { get; set; }
        public bool CanDelete { get; set; }
        public bool CanReadFiles { get; set; }
        public int? MilestoneId { get; set; }
        public List<Subtask> Subtasks { get; set; } = [];
        public UserInfo UpdatedBy { get; set; }
        public int? Progress { get; set; }
        public List<UserInfo> Responsibles { get; set; } = [];
        public DateTime? Deadline { get; set; }
        public DateTime? StartDate { get; set; }
        public int? TaskStatusId { get; set; }
        public Status Status { get; set; }
    }
}
