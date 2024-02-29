using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public sealed record class OnlyofficeTask : AuditableEntityBase<int>
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

        public bool Equals(OnlyofficeTask other)
        {
            if (other is null) return false;

            return Id == other.Id && 
                Title == other.Title &&
                Description == other.Description &&
                TaskStatusId == other.TaskStatusId &&
                Status == other.Status &&
                Deadline == other.Deadline &&
                StartDate == other.StartDate &&
                Progress == other.Progress && 
                Priority == other.Priority &&
                MilestoneId == other.MilestoneId;
        }
        
        public override int GetHashCode() => Id;

        public override string ToString()
        {
            return $"OnlyofficeTask - {Id}\t{Title}";
        }
    }
}
