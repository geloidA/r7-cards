using Onlyoffice.Api.Common;

namespace Cardmngr.Models;

public class TaskModel(Onlyoffice.Api.Models.Task task, ProjectModel project) : ModelBase
{
    public bool CanEdit { get; } = task.CanEdit;
    public bool CanCreateSubtask { get; } = task.CanCreateSubtask;
    public bool CanCreateTimeSpend { get; } = task.CanCreateTimeSpend;
    public bool CanDelete { get; } = task.CanDelete;
    public bool CanReadFiles { get; } = task.CanReadFiles;
    public int Id { get; } = task.Id;
    public string? Title { get; set; } = task.Title;
    public string? Description { get; set; } = task.Description;
    public int Priority { get; set; } = task.Priority;
    public MilestoneModel? Milestone { get; set; } = project.Milestones.FirstOrDefault(m => m.Id == task.MilestoneId);
    public ProjectModel ProjectOwner { get; } = project;
    public List<SubtaskModel>? Subtasks { get; set; } = task.Subtasks?.Select(s => new SubtaskModel(s)).ToList();
    public Status Status { get; set; } = (Status)task.Status;
    public int? Progress { get; set; } = task.Progress;
    public User? UpdatedBy { get; } = task.UpdatedBy == null ? null : new User(task.UpdatedBy!);
    public DateTime Created { get; } = task.Created;
    public User CreatedBy { get; } = new User(task.CreatedBy!);
    public DateTime Updated { get; } = task.Updated;
    public List<User>? Responsibles { get; set; } = task.Responsibles?.Select(u => new User(u)).ToList();
    public int? CustomTaskStatus { get; set; } = task.CustomTaskStatus;
    public DateTime? Deadline { get; set; } = task.Deadline;
    public DateTime? StartDate { get; set; } = task.StartDate;
}
