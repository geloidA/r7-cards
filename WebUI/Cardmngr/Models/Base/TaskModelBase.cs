using System.ComponentModel.DataAnnotations;
using Cardmngr.Models.Attributes;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models.Base;

public abstract class TaskModelBase<TApiModel> : EditableModelBase<TApiModel>, ITaskModel
{
    public TaskModelBase(IStatusColumnModel statusColumn)
    {
        Project = statusColumn.Project;
        this.statusColumn = statusColumn;
    }

    public TaskModelBase(TApiModel task, IStatusColumnModel statusColumn) : this(statusColumn)
    {
        Update(task);
    }

    [Updatable]
    public bool CanCreateSubtask { get; protected set; }

    [Updatable]
    public bool CanCreateTimeSpend { get; protected set; }

    [Updatable]
    public bool CanReadFiles { get; protected set; }

    [Updatable]
    public int? Progress { get; set; }

    [Updatable]
    public IUser? UpdatedBy { get; protected set; }

    public abstract IEnumerable<IUser> Responsibles { get; }

    public abstract IEnumerable<ISubtaskModel> Subtasks { get; }

    public IMilestoneModel? Milestone { get; set; }

    public IProjectModel Project { get; }

    private IStatusColumnModel statusColumn;
    public IStatusColumnModel StatusColumn 
    {
        get => statusColumn;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            statusColumn.Remove(this);
            value.Add(this);

            statusColumn = value;
        }
    }

    [Updatable]
    public DateTime? StartDate { get; set; }

    [Updatable]
    [CustomValidation(typeof(DeadlineValidation), nameof(DeadlineValidation.CheckDeadline))]
    public DateTime? Deadline { get; set; }

    [Updatable]
    public TaskPriority Priority { get; set; }

    public bool IsClosed() => StatusColumn.Status == Status.Closed;

    public abstract void AddRangeResponsibles(params IUser[] responsibles);
    public abstract void AddSubtask(ISubtaskModel subtask);
    public abstract void ClearResponsibles();
    public abstract bool RemoveSubtask(ISubtaskModel subtask);
}
