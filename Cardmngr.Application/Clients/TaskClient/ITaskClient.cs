using Cardmngr.Application.Clients.Base;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.TaskClient;

public interface ITaskClient : IEntityClient<OnlyofficeTask, TaskUpdateData>
{
    Task<OnlyofficeTask> UpdateTaskStatusAsync(int taskId, OnlyofficeTaskStatus status);

    IAsyncEnumerable<OnlyofficeTask> GetSelfTasksAsync();

    IAsyncEnumerable<Comment> GetTaskCommentsAsync(int taskId);

    IAsyncEnumerable<TaskTag> GetTaskTagsAsync(int taskId);

    Task<TaskTag> CreateTagAsync(int taskId, string name);

    Task RemoveTagAsync(string tagId);
}
