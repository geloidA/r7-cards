using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.People;

/// <summary>
/// Клиент для работы с людьми.
/// </summary>
public interface IPeopleClient
{
    /// <summary>
    /// Получает всех пользователей.
    /// </summary>
    /// <returns>пользователи.</returns>
    IAsyncEnumerable<UserProfile> GetUsersAsync();

    /// <summary>
    /// Получает пользователей, подходящих под фильтр.
    /// </summary>
    /// <param name="filterBuilder">фильтр для поиска пользователей.</param>
    /// <returns>пользователи, подходящие под фильтр.</returns>
    IAsyncEnumerable<UserProfile> GetFilteredUsersAsync(FilterBuilder filterBuilder);

    /// <summary>
    /// Получает профиль пользователя по его идентификатору.
    /// </summary>
    /// <param name="userId">идентификатор пользователя.</param>
    /// <returns>профиль пользователя.</returns>
    Task<UserProfile> GetUserProfileByIdAsync(string userId);
}
