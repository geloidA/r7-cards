using BlazorCards;

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

public class Task : ICardDao
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
    public TaskOwner? ProjectOwner { get; set; }
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

    public Task FullCopy(bool copySubtasks = true)
    {
        return new Task
        {
            CanEdit = CanEdit,
            CanCreateSubtask = CanCreateSubtask,
            CanDelete = CanDelete,
            CanReadFiles = CanReadFiles,
            Id = Id,
            Title = Title,
            Description = Description,
            Priority = Priority,
            MilestoneId = MilestoneId,
            Milestone = Milestone?.FullCopy(),
            ProjectOwner = ProjectOwner?.FullCopy(),
            Subtasks = copySubtasks ? Subtasks?.Select(x => x.FullCopy()).ToList() : Subtasks,
            Status = Status,
            Progress = Progress,
            UpdatedBy = UpdatedBy?.FullCopy(),
            Created = Created,
            CreatedBy = CreatedBy?.FullCopy(),
            Updated = Updated,
            Responsibles = Responsibles?.Select(x => x.FullCopy()).ToList(),
            CustomTaskStatus = CustomTaskStatus,
            Deadline = Deadline,
            StartDate = StartDate
        };
    }

    public UpdatedStateTask GetUpdateState()
    {
        return new UpdatedStateTask
        {
            Description = Description,
            Deadline = Deadline,
            StartDate = StartDate,
            Priority = Priority,
            Title = Title,
            MilestoneId = MilestoneId,
            Responsibles = Responsibles!.Select(x => x.Id).ToList(),
            ProjectId = ProjectOwner!.Id,
            Progress = Progress
        };
    }
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

public class TaskOwner
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public bool IsPrivate { get; set; }

    public TaskOwner FullCopy()
    {
        return new TaskOwner
        {
            Id = Id,
            Title = Title,
            Status = Status,
            IsPrivate = IsPrivate
        };
    }
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

    public UpdatedStateSubtask GetUpdatedState()
    {
        return new UpdatedStateSubtask
        {
            Title = Title,
            Responsible = Responsible?.Id
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Subtask other) return false;
        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public Subtask FullCopy()
    {
        return new Subtask
        {
            CanEdit = CanEdit,
            TaskId = TaskId,
            Id = Id,
            Title = Title,
            Description = Description,
            Status = Status,
            Created = Created,
            CreatedBy = CreatedBy?.FullCopy(),
            Updated = Updated,
            Responsible = Responsible?.FullCopy()
        };
    }
}

public class UpdatedStateSubtask
{
    public string? Responsible { get; set; }
    public string? Title { get; set; }
}

public class CreatedBy
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }

    public CreatedBy FullCopy()
    {
        return new CreatedBy
        {
            Id = Id,
            DisplayName = DisplayName,
            AvatarSmall = AvatarSmall,
            ProfileUrl = ProfileUrl
        };
    }
}

public class UpdatedBy
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }

    public UpdatedBy FullCopy()
    {
        return new UpdatedBy
        {
            Id = Id,
            DisplayName = DisplayName,
            AvatarSmall = AvatarSmall,
            ProfileUrl = ProfileUrl
        };
    }
}
