namespace Cardmngr.Network.Models;

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
    public TaskOwner? ProjectOwner { get; set; }
    public List<Subtask>? Subtasks { get; set; }
    public int Status { get; set; }
    public UpdatedBy? UpdatedBy { get; set; }
    public DateTime Created { get; set; }
    public CreatedBy? CreatedBy { get; set; }
    public DateTime Updated { get; set; }
    public List<Responsible>? Responsibles { get; set; }
}

public class TaskOwner
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public bool IsPrivate { get; set; }
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
}

public class CreatedBy
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }
}

public class UpdatedBy
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }
}
