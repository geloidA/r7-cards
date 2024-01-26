using System.Collections;
using Onlyoffice.Api.Models.Extensions;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Models;

public class StatusColumnsModel(List<MyTask> tasks, List<MyTaskStatus> statuses, ProjectModel project) : ModelBase, IEnumerable<TaskStatusColumn>
{
    private readonly List<TaskStatusColumn> statusColumns = statuses
        .OrderBy(x => (x.StatusType, x.Order))
        .Select(x => new TaskStatusColumn(x, tasks.FilterByStatus(x), project))
        .ToList();

    public TaskModel? LastDraggedTask { get; private set; }

    public void StartDrag(TaskModel task)
    {
        if (!statusColumns.Contains(task.StatusColumn))
            throw new InvalidOperationException("Task does not belong to this column");

        LastDraggedTask = task;
    }

    public IEnumerator<TaskStatusColumn> GetEnumerator() => statusColumns.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
