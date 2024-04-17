using System.ComponentModel.DataAnnotations;

namespace Onlyoffice.Api.Models;

public class SingleSubtaskDao : HttpResponseDaoBase
{
    public SubtaskDto? Response { get; set; }
}

public class SingleTaskDao : HttpResponseDaoBase
{
    public TaskDto? Response { get; set; }
}

public class TaskDao : HttpResponseDaoBase
{
    public List<TaskDto>? Response { get; set; }
}

public class TaskDto
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
    public MilestoneDto? Milestone { get; set; }
    public ProjectInfoDto? ProjectOwner { get; set; }
    public List<SubtaskDto>? Subtasks { get; set; }
    public int Status { get; set; }
    public int? Progress { get; set; }
    public UserDto? UpdatedBy { get; set; }
    public DateTime Created { get; set; }
    public UserDto? CreatedBy { get; set; }
    public DateTime Updated { get; set; }
    public List<UserDto>? Responsibles { get; set; }
    public int? CustomTaskStatus { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? StartDate { get; set; }
}

public class TaskUpdateData
{
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? StartDate { get; set; }
    public int Priority { get; set; }
    public string? Title { get; set; }
    public int? MilestoneId { get; set; }
    public List<string> Responsibles { get; set; } = [];
    public int? ProjectId { get; set; }
    public bool? Notify { get; set; }
    public int? Status { get; set; } = (int)Common.Status.Open;
    public int? Progress { get; set; }
    public int? CustomTaskStatus { get; set; }
}

public class SubtaskDto
{
    public bool CanEdit { get; set; }
    public int TaskId { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public DateTime Created { get; set; }
    public UserDto? CreatedBy { get; set; }
    public DateTime Updated { get; set; }
    public UserDto? Responsible { get; set; }
}

public class SubtaskUpdateData
{
    public int Status { get; set; } = (int)Common.Status.Open;
    public string? Responsible { get; set; }

    [Required(ErrorMessage = "Название не может быть пустым")]
    public string? Title { get; set; }
}

public class UserDto : IUser
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }
}
