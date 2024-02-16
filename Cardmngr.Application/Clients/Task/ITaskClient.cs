using Cardmngr.Domain.Entities;

namespace Cardmngr.Application.Clients;

public interface ITaskClient
{
    Task UpdateTaskStatusAsync(int taskId, OnlyofficeTaskStatus status);
    Task<OnlyofficeTask> CreateTaskAsync(int projectId, OnlyofficeTask task);
    Task<OnlyofficeTask> UpdateTaskAsync(int projectId, OnlyofficeTask task);
    Task RemoveTaskAsync(int taskId);
}
