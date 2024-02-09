using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models.Base;

#warning Doen't have CanDelete property
public abstract class SubtaskModelBase : ModelBase, ISubtaskModel
{
    public SubtaskModelBase(Status status)
    {
        Status = status;
    }
    
    public SubtaskModelBase(Subtask subtask)
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

    public int TaskId { get; protected set; }

    public Status Status { get; private set; }

    public IUser? Responsible { get; set; }

    public void Close()
    {
        Status = Status.Closed;
    }

    public void Open()
    {
        Status = Status.Open;
    }
}
