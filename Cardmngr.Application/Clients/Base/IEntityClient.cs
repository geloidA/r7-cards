using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.Base;

public interface IEntityClient<TEntity, TUpdateData>
{
    /// <summary>
    /// Создает сущность.
    /// </summary>
    /// <param name="projectId">идентификатор проекта, к которому будет добавлена сущность.</param>
    /// <param name="updateData">данные для создания сущности.</param>
    /// <returns>созданная сущность.</returns>
    Task<TEntity> CreateAsync(int projectId, TUpdateData updateData);

    /// <summary>
    /// Обновляет сущность.
    /// </summary>
    /// <param name="entityId">идентификатор сущности, которую нужно обновить.</param>
    /// <param name="updateData">данные для обновления сущности.</param>
    /// <returns>обновленная сущность.</returns>
    Task<TEntity> UpdateAsync(int entityId, TUpdateData updateData);

    /// <summary>
    /// Удаляет сущность.
    /// </summary>
    /// <param name="entityId">идентификатор сущности, которую нужно удалить.</param>
    /// <returns>удаленная сущность.</returns>
    Task<TEntity> RemoveAsync(int entityId);

    /// <summary>
    /// Получает сущность.
    /// </summary>
    /// <param name="entityId">идентификатор сущности, которую нужно получить.</param>
    /// <returns>сущность.</returns>
    Task<TEntity> GetAsync(int entityId);

    /// <summary>
    /// Получает все сущности асинхронно.
    /// </summary>
    /// <param name="filterBuilder">построитель фильтра для выборки сущностей (необязательно).</param>
    /// <returns>поток сущностей.</returns>
    IAsyncEnumerable<TEntity> GetEntitiesAsync(FilterBuilder? filterBuilder = null);
}
