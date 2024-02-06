using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

#warning Doen't have CanDelete property
public class SubtaskModel : ModelBase
{
    public SubtaskModel(Subtask subtask)
    {
        CanEdit = subtask.CanEdit;
        TaskId = subtask.TaskId;
        Id = subtask.Id;
        Title = subtask.Title ?? subtask.Id.ToString();
        Description = subtask.Description;
        Status = (Status)subtask.Status;
        Created = subtask.Created;
        CreatedBy = new User(subtask.CreatedBy!);
        Updated = subtask.Updated;
        Responsible = subtask.Responsible != null ? new User(subtask.Responsible) : null;
    }

    private SubtaskModel(SubtaskModel source)
    {
        CanEdit = source.CanEdit;
        TaskId = source.TaskId;
        Id = source.Id;
        Title = source.Title;
        Description = source.Description;
        Status = source.Status;
        Created = source.Created;
        CreatedBy = source.CreatedBy;
    }
    
    public int TaskId { get; }
    public int Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public IUser? Responsible { get; set; }
    
    public SubtaskModel Clone() => new(this);
}
