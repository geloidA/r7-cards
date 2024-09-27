using Cardmngr.Domain.Enums;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.Subtask;

/// <summary>
/// Клиент для работы с подзадачами.
/// </summary>
public interface ISubtaskClient
{
    /// <summary>
    /// Создает подзадачу.
    /// </summary>
    /// <param name="taskId">идентификатор задачи, к которой будет добавлена подзадача.</param>
    /// <param name="updateData">данные для создания подзадачи.</param>
    /// <returns>созданная подзадача.</returns>
    Task<Domain.Entities.Subtask> CreateAsync(int taskId, SubtaskUpdateData updateData);

    /// <summary>
    /// Обновляет подзадачу.
    /// </summary>
    /// <param name="taskId">идентификатор задачи, к которой относится обновляемая подзадача.</param>
    /// <param name="subtaskId">идентификатор подзадачи, которую нужно обновить.</param>
    /// <param name="updateData">данные для обновления подзадачи.</param>
    /// <returns>обновленная подзадача.</returns>
    Task<Domain.Entities.Subtask> UpdateAsync(int taskId, int subtaskId, SubtaskUpdateData updateData);

    /// <summary>
    /// Удаляет подзадачу.
    /// </summary>
    /// <param name="taskId">идентификатор задачи, к которой относится удаляемая подзадача.</param>
    /// <param name="subtaskId">идентификатор подзадачи, которую нужно удалить.</param>
    /// <returns>удаленная подзадача.</returns>
    Task<Domain.Entities.Subtask> RemoveAsync(int taskId, int subtaskId);

    /// <summary>
    /// Обновляет статус подзадачи.
    /// </summary>
    /// <param name="taskId">идентификатор задачи, к которой относится обновляемая подзадача.</param>
    /// <param name="subtaskId">идентификатор подзадачи, которую нужно обновить.</param>
    /// <param name="status">новый статус подзадачи.</param>
    /// <returns>обновленная подзадача.</returns>
    Task<Domain.Entities.Subtask> UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status);
}
