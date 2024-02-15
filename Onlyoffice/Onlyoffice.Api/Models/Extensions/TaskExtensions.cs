namespace Onlyoffice.Api.Models.Extensions;

public static class TaskExtensions
{
    public static IEnumerable<Task> FilterByStatus(this IEnumerable<Task> tasks, TaskStatus status) // TODO: move somewhere
    {
        return tasks
            .Where(t => t.CustomTaskStatus.HasValue 
                ? t.CustomTaskStatus.Value == status.Id 
                : t.Status == status.StatusType && status.IsDefault);
    }
}
