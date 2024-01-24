using BlazorCards.Core;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Utils;

public static class ModelConverter
{
    private static IEnumerable<IEnumerable<MyTask>> DivideOnStatues(
        IEnumerable<MyTaskStatus> statuses,
        IEnumerable<MyTask> tasks)
    {
        return statuses
            .Select(x => tasks
                .Where(t => t.CustomTaskStatus.HasValue ? t.CustomTaskStatus.Value == x.Id : 
                    t.Status == x.StatusType && x.IsDefault)
                .OrderBy(x => (-x.Priority, x.Deadline ?? DateTime.MaxValue))
                .ThenByDescending(x => x.Updated));
    }

    public static IEnumerable<MyTask> FilterByStatus(this IEnumerable<MyTask> tasks, MyTaskStatus status)
    {
        return tasks
            .Where(t => t.CustomTaskStatus.HasValue 
                ? t.CustomTaskStatus.Value == status.Id 
                : t.Status == status.StatusType && status.IsDefault);
    }
}
