using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public record class OnlyofficeTask : AuditableEntityBase<int>, IEquatable<OnlyofficeTask>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public Priority Priority { get; init; }
        public bool CanEdit { get; init; } 
        public bool CanCreateSubtask { get; init; }
        public bool CanCreateTimeSpend { get; init; }
        public bool CanDelete { get; init; }
        public bool CanReadFiles { get; init; }
        public int? MilestoneId { get; init; }
        public List<Subtask> Subtasks { get; init; } = [];
        public UserInfo UpdatedBy { get; init; }
        public int? Progress { get; init; }
        public List<UserInfo> Responsibles { get; init; } = [];
        public DateTime? Deadline { get; init; }
        public DateTime? StartDate { get; init; }
        public int? TaskStatusId { get; init; }
        public Status Status { get; init; }

        public override string ToString()
        {
            return $"OnlyofficeTask - {Id}\t{Title}";
        }
    }
}
