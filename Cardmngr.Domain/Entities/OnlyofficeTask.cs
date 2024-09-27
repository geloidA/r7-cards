using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities;

public sealed record OnlyofficeTask : AuditableEntityBase<int>, IOnlyofficeTask
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
    public ProjectInfo ProjectOwner { get; init; }
    public MilestoneInfo Milestone { get; init; }
    public List<TaskTag> Tags { get; set; } = [];

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
            MilestoneId == other.MilestoneId &&
            ProjectOwner == other.ProjectOwner;
    }
    
    public override int GetHashCode() => Id;

    public override string ToString()
    {
        return $"OnlyofficeTask - {Id}\t{Title}";
    }
}

public interface IOnlyofficeTask 
{
    int Id { get; }
    string Title { get; }
    DateTime? StartDate { get; }
    DateTime? Deadline { get; }
    int? TaskStatusId { get; }
    Status Status { get; }
    int? MilestoneId { get; }
}
