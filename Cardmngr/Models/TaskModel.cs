using Cardmngr.Models.Base;
using Cardmngr.Models.EventArgs;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

public class TaskModel : TaskModelBase<Onlyoffice.Api.Models.Task>
{
    private readonly List<ISubtaskModel> subtasks;
    private List<IUser> responsibles;

    public TaskModel(Onlyoffice.Api.Models.Task task, IStatusColumnModel statusColumn) : base(task, statusColumn)
    {
        subtasks = new List<ISubtaskModel>(task.Subtasks?.Select(s => new SubtaskModel(s)) ?? []);
        responsibles = new List<IUser>(task.Responsibles?.Select(u => new User(u)) ?? []);

        Project.Milestones.CollectionChanged += UnsetIfRemoved;
    }

    private void UnsetIfRemoved(CollectionEventArgs<IMilestoneModel> args)
    {
        if (args.Action == CollectionAction.Remove)
        {
            if (Milestone?.Id == args.Item.Id)
            {
                Milestone = null;
                OnModelChanged();
            }
        }
    }

    private TaskModel(TaskModel source) : base(source.StatusColumn)
    {
        CanEdit = source.CanEdit;
        CanDelete = source.CanDelete;
        Id = source.Id;
        Title = source.Title;
        Description = source.Description;
        Priority = source.Priority;
        Milestone = source.Milestone; // Use the same milestone
        subtasks = source.subtasks;
        Progress = source.Progress;

        responsibles = source.Responsibles.Select(u => new User(u)).ToList<IUser>();

        Deadline = source.Deadline;
        StartDate = source.StartDate;
    }

    public override IEditableModel<Onlyoffice.Api.Models.Task> EditableModel => new TaskModel(this);

    public override void Update(Onlyoffice.Api.Models.Task source)
    {
        Milestone = Project.Milestones.FirstOrDefault(m => m.Id == source.Milestone?.Id);
        responsibles = new List<IUser>(source.Responsibles?.Select(u => new User(u)) ?? []);

        base.Update(source);
    }

    public override IEnumerable<IUser> Responsibles
    {
        get
        {
            foreach (var responsible in responsibles)
                yield return responsible;
        }
    }

    public override IEnumerable<ISubtaskModel> Subtasks
    {
        get
        {
            foreach (var subtask in subtasks)
                yield return subtask;
        }
    }

    public void CloseAllSubtasks()
    {
        if (Subtasks is not { } || !Subtasks.Any())
            throw new InvalidOperationException("No subtasks found");

        foreach (var subtask in Subtasks)
        {
            subtask.Close();
        }
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TaskModel other) return false;
        return other.Id == Id;
    }

    public override void AddRangeResponsibles(params IUser[] responsibles)
    {
        this.responsibles.AddRange(responsibles);
        OnModelChanged();
    }

    public override void AddSubtask(ISubtaskModel subtask)
    {
        subtasks.Add(subtask);
        OnModelChanged();
    }

    public override void ClearResponsibles()
    {
        responsibles.Clear();
        OnModelChanged();
    }

    public override bool RemoveSubtask(ISubtaskModel subtask)
    {
        if (subtasks.Remove(subtask))
        {
            OnModelChanged();
            return true;
        }
        return false;
    }
}
