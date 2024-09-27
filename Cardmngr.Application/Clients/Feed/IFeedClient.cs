using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.Feed;

/// <summary>
/// Клиент для работы с лентой.
/// </summary>
public interface IFeedClient
{
    /// <summary>
    /// Получает элементы ленты, отфильтрованные по переданному фильтру.
    /// </summary>
    /// <param name="filterBuilder">фильтр, по которому нужно отфильтровать элементы.</param>
    /// <returns>отфильтрованные элементы.</returns>
    IAsyncEnumerable<Domain.Entities.Feed> GetFiltredAsync(FilterBuilder filterBuilder);
}
