using Cardmngr.Extensions;
using Cardmngr.Models.Base;
using Cardmngr.Models.EventArgs;
using Cardmngr.Models.Interfaces;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Models;

public class StatusColumn : StatusColumnModelBase
{
    private readonly List<ITaskModel> tasks;

    public StatusColumn(MyTaskStatus taskStatus, IEnumerable<Onlyoffice.Api.Models.Task> tasks, IProjectModel projectModel)
        : base(taskStatus, projectModel)
    {
        this.tasks = tasks
            .Select(x => new TaskModel(x, this))
            .ToList<ITaskModel>();
    }

    public override int Count => tasks.Count;

    public override void Add(ITaskModel task)
    {
        if (tasks.Contains(task))
            throw new InvalidOperationException("Task already exists");

        tasks.Add(task);

        OnCollectionChanged(task, CollectionAction.Add);
        OnModelChanged();
    }

    public override IEnumerator<ITaskModel> GetEnumerator()
    {
        foreach (var task in tasks.OrderByProperties())
            yield return task;
    }

    public override bool Remove(ITaskModel task)
    {
        if (tasks.Remove(task))
        {
            OnCollectionChanged(task, CollectionAction.Remove);
            OnModelChanged();
            return true;
        }
        return false;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not StatusColumn other)
            return false;
        return other.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}