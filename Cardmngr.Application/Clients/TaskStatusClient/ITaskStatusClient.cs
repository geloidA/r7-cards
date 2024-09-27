using Cardmngr.Domain.Entities;

namespace Cardmngr.Application.Clients.TaskStatusClient;

public interface ITaskStatusClient
{
    /// <summary>
    /// Получает список всех статусов задач.
    /// </summary>
    /// <returns>список статусов задач.</returns>
    IAsyncEnumerable<OnlyofficeTaskStatus> GetAllAsync();
}
