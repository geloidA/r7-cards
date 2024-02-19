namespace Onlyoffice.Api.Models.Extensions;

public static class TaskExtensions
{
    public static IEnumerable<TaskDto> FilterByStatus(this IEnumerable<TaskDto> tasks, TaskStatusDto status) // TODO: move somewhere
    {
        return tasks
            .Where(t => t.CustomTaskStatus.HasValue 
                ? t.CustomTaskStatus.Value == status.Id 
                : t.Status == status.StatusType && status.IsDefault);
    }
}
