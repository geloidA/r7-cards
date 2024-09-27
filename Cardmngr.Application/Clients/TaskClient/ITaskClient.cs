using Cardmngr.Application.Clients.Base;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.TaskClient;

/// <summary>
/// Клиент для работы с задачами.
/// </summary>
public interface ITaskClient : IEntityClient<OnlyofficeTask, TaskUpdateData>
{
    /// <summary>
    /// Обновляет статус задачи.
    /// </summary>
    /// <param name="taskId">идентификатор задачи.</param>
    /// <param name="status">новый статус задачи.</param>
    /// <returns>обновленная задача.</returns>
    Task<OnlyofficeTask> UpdateTaskStatusAsync(int taskId, OnlyofficeTaskStatus status);

    /// <summary>
    /// Получает список задач, в которых пользователь является автором.
    /// </summary>
    /// <returns>список задач, в которых пользователь является автором.</returns>
    IAsyncEnumerable<OnlyofficeTask> GetSelfTasksAsync();

    /// <summary>
    /// Получает список комментариев к задаче.
    /// </summary>
    /// <param name="taskId">идентификатор задачи.</param>
    /// <returns>список комментариев к задаче.</returns>
    IAsyncEnumerable<Comment> GetTaskCommentsAsync(int taskId);

    /// <summary>
    /// Получает список тегов задачи.
    /// </summary>
    /// <param name="taskId">идентификатор задачи.</param>
    /// <returns>список тегов задачи.</returns>
    IAsyncEnumerable<TaskTag> GetTaskTagsAsync(int taskId);

    /// <summary>
    /// Создает новый тег задачи.
    /// </summary>
    /// <param name="taskId">идентификатор задачи.</param>
    /// <param name="name">название тега.</param>
    /// <returns>созданный тег.</returns>
    Task<TaskTag> CreateTagAsync(int taskId, string name);

    /// <summary>
    /// Удаляет тег задачи.
    /// </summary>
    /// <param name="tagId">идентификатор тега.</param>
    Task RemoveTagAsync(string tagId);
}
