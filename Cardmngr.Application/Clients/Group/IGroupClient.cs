namespace Cardmngr.Application.Clients.Group;

/// <summary>
/// Клиент для работы с группами.
/// </summary>
public interface IGroupClient
{
    /// <summary>
    /// Получает список групп.
    /// </summary>
    /// <returns>список групп.</returns>
    IAsyncEnumerable<Domain.Entities.Group> GetGroupsAsync();
}
