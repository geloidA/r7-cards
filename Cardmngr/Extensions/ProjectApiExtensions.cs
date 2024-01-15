using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Extensions;

public static class ProjectApiExtensions
{
    public static async Task UpdateTaskStatusWithCheckAsync(this IProjectApi api, Onlyoffice.Api.Models.Task task, Status status, int? customStatus = null)
    {
        if ((Status)task.Status == status && task.CustomTaskStatus == customStatus) return;

        await api.UpdateTaskStatusAsync(task.Id, status, customStatus);
    }
}
