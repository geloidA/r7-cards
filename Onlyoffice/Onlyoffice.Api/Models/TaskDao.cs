﻿using System.ComponentModel.DataAnnotations;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;

namespace Onlyoffice.Api.Models;

public class SingleSubtaskDao : SingleResponseDao<SubtaskDto>;

public class SingleTaskDao : SingleResponseDao<TaskDto>;

public class TaskDao : MultiResponseDao<TaskDto>;

public class TaskDto : IEntityDto<int>
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

public sealed class TaskUpdateData : IOnlyofficeTask
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

    public int Id { get; set; }

    public int? TaskStatusId { get; set; }

    Status IOnlyofficeTask.Status
    {
        get 
        {
            return Status.HasValue ? (Status)Status.Value : Cardmngr.Domain.Enums.Status.Open;            
        }
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(Status)}: {Status}, {nameof(Description)}: {Description}, {nameof(Deadline)}: {Deadline}, {nameof(StartDate)}: {StartDate}, {nameof(Priority)}: {Priority}, {nameof(MilestoneId)}: {MilestoneId}, {nameof(Responsibles)}: {string.Join(", ", Responsibles)}, {nameof(ProjectId)}: {ProjectId}, {nameof(Notify)}: {Notify}, {nameof(TaskStatusId)}: {TaskStatusId}";
    }
}

public class SubtaskDto : IEntityDto<int>
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

public class UserDto : IUser, IEntityDto<string?>
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }
}
