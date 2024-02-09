using System.Collections;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Models.Extensions;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Models;

public class StatusColumnsModel(IEnumerable<MyTaskStatus> statuses, 
    IEnumerable<MyTask> tasks,
    IProjectModel project) : IStatusColumnBoard
{
    private readonly List<IStatusColumnModel> statusColumns = statuses
        .OrderBy(x => (x.StatusType, x.Order))
        .Select(x => new StatusColumn(x, tasks.FilterByStatus(x), project))
        .ToList<IStatusColumnModel>();

    public ITaskModel? LastDraggedTask { get; private set; }

    public IEnumerator<IStatusColumnModel> GetEnumerator()
    {
        foreach (var statusColumn in statusColumns)
            yield return statusColumn;
    }

    public void StartDrag(ITaskModel task)
    {
        if (!statusColumns.Contains(task.StatusColumn))
            throw new InvalidOperationException("Task does not belong to this column");

        LastDraggedTask = task;
    }

    public void UnsetDraggedTask() => LastDraggedTask = null;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
