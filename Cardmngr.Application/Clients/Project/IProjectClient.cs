using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.Project;

/// <summary>
/// Клиент для работы с проектами.
/// </summary>
public interface IProjectClient
{
    /// <summary>
    /// Получает состояние проекта.
    /// </summary>
    /// <param name="projectId">идентификатор проекта, состояние которого нужно получить.</param>
    /// <returns>состояние проекта.</returns>
    Task<ProjectStateDto> GetProjectStateAsync(int projectId);

    /// <summary>
    /// Получает проект.
    /// </summary>
    /// <param name="projectId">идентификатор проекта, который нужно получить.</param>
    /// <returns>проект.</returns>
    Task<Domain.Entities.Project> GetProjectAsync(int projectId);

    /// <summary>
    /// Удаляет проект.
    /// </summary>
    /// <param name="projectId">идентификатор проекта, который нужно удалить.</param>
    /// <returns>удаленный проект.</returns>
    Task<Domain.Entities.Project> DeleteProjectAsync(int projectId);

    /// <summary>
    /// Получает проекты, которые принадлежат пользователю.
    /// </summary>
    /// <returns>проекты, которые принадлежат пользователю.</returns>
    IAsyncEnumerable<Domain.Entities.Project> GetSelfProjectsAsync();

    /// <summary>
    /// Получает проекты, на которые подписан пользователь.
    /// </summary>
    /// <returns>проекты, на которые подписан пользователь.</returns>
    IAsyncEnumerable<Domain.Entities.Project> GetFollowedProjectsAsync();

    /// <summary>
    /// Получает проекты, которые содержат задачи, созданные пользователем.
    /// </summary>
    /// <returns>проекты, которые содержат задачи, созданные пользователем.</returns>
    IAsyncEnumerable<ProjectStateDto> GetProjectsWithSelfTasksAsync();

    /// <summary>
    /// Получает все проекты.
    /// </summary>
    /// <returns>все проекты.</returns>
    IAsyncEnumerable<Domain.Entities.Project> GetProjectsAsync();

    /// <summary>
    /// Получает проекты, которые содержат задачи, созданные пользователем, сгруппированные по проекту.
    /// </summary>
    /// <param name="filter">фильтр для поиска проектов.</param>
    /// <returns>проекты, которые содержат задачи, созданные пользователем, сгруппированные по проекту.</returns>
    IAsyncEnumerable<KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>> GetGroupedFilteredTasksAsync(FilterBuilder filter);

    /// <summary>
    /// Формирует проект с задачами.
    /// </summary>
    /// <param name="tasks">задачи, которые нужно добавить к проекту.</param>
    /// <returns>проект, к которому были добавлены задачи.</returns>
    Task<ProjectStateDto> CollectProjectWithTasksAsync(ICollection<OnlyofficeTask> tasks);

    /// <summary>
    /// Cоздает проект.
    /// </summary>
    /// <param name="project">данные для создания проекта.</param>
    /// <returns>созданный проект.</returns>
    Task<Domain.Entities.Project> CreateProjectAsync(ProjectCreateDto project);

    /// <summary>
    /// Подписывает пользователя на проект.
    /// </summary>
    /// <param name="projectId">идентификатор проекта, на который нужно подписаться.</param>
    /// <returns>проект, на который подписался пользователь.</returns>
    Task<Domain.Entities.Project> FollowProjectAsync(int projectId);
}
