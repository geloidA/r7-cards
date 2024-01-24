using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

public class SubtaskModel(Subtask subtask) : ModelBase
{
    public bool CanEdit { get; } = subtask.CanEdit;
    public int TaskId { get; } = subtask.TaskId;
    public int Id { get; } = subtask.TaskId;
    public string? Title { get; set; } = subtask.Title;
    public string? Description { get; set; } = subtask.Description;
    public Status Status { get; set; } = (Status)subtask.Status;
    public DateTime Created { get; } = subtask.Created;
    public User CreatedBy { get; } = new User(subtask.CreatedBy!);
    public DateTime Updated { get; } = subtask.Updated;
    public User? Responsible { get; } = subtask.Responsible != null ? new User(subtask.Responsible) : null;
}
