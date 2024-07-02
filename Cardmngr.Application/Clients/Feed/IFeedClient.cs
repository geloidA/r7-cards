using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.Feed;

public interface IFeedClient
{
    IAsyncEnumerable<Domain.Entities.Feed> GetFiltredAsync(FilterBuilder filterBuilder);
}
