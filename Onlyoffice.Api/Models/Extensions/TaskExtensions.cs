using Onlyoffice.Api.Common;

namespace Onlyoffice.Api.Models.Extensions;

public static class TaskExtensions
{
    public static bool CanMarkClosed(this Task task)
    {
        return task.Status == (int)Status.Closed
            || (task.Subtasks?.All(x => x.Status == (int)Status.Closed) ?? false);
    }
}
