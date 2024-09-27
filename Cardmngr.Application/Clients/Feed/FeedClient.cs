using AutoMapper;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.Feed;

/// <summary>
/// Клиент для работы с лентой.
/// </summary>
public class FeedClient(IFeedRepository feedRepository, IMapper mapper) : IFeedClient
{
    public IAsyncEnumerable<Domain.Entities.Feed> GetFiltredAsync(FilterBuilder filterBuilder)
    {
        return feedRepository
            .GetFiltredAsync(filterBuilder)
            .Select(mapper.Map<Domain.Entities.Feed>);
    }
}
