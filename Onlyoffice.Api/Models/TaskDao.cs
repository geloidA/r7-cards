namespace Onlyoffice.Api.Models;

public class SingleSubtaskDao : HttpResponseDaoBase
{
    public Subtask? Response { get; set; }
}

public class SingleTaskDao : HttpResponseDaoBase
{
    public Task? Response { get; set; }
}

public class TaskDao : HttpResponseDaoBase
{
    public List<Task>? Response { get; set; }
}

public class Task
{
    public bool CanEdit { get; set; }
    public bool CanCreateSubtask { get; set; }
    public bool CanCreateTimeSpend { get; set; }
    public bool CanDelete { get; set; }
    public bool CanReadFiles { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; }
    public int? MilestoneId { get; set; }
    public Milestone? Milestone { get; set; }
    public ProjectInfo? ProjectOwner { get; set; }
    public List<Subtask>? Subtasks { get; set; }
    public int Status { get; set; }
    public int? Progress { get; set; }
    public UpdatedBy? UpdatedBy { get; set; }
    public DateTime Created { get; set; }
    public CreatedBy? CreatedBy { get; set; }
    public DateTime Updated { get; set; }
    public List<Responsible>? Responsibles { get; set; }
    public int? CustomTaskStatus { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? StartDate { get; set; }
}

public class UpdatedStateTask
{
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? StartDate { get; set; }
    public int Priority { get; set; }
    public string? Title { get; set; }
    public int? MilestoneId { get; set; }
    public List<string?>? Responsibles { get; set; }
    public int? ProjectId { get; set; }
    public bool? Notify { get; set; }
    public int? Status { get; set; }
    public int? Progress { get; set; }
}

public class Subtask
{
    public bool CanEdit { get; set; }
    public int TaskId { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public DateTime Created { get; set; }
    public CreatedBy? CreatedBy { get; set; }
    public DateTime Updated { get; set; }
    public Responsible? Responsible { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Subtask other) return false;
        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public class UpdatedStateSubtask
{
    public string? Responsible { get; set; }
    public string? Title { get; set; }
}

public class CreatedBy : IUser
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }
}

public class UpdatedBy : IUser
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }
}
