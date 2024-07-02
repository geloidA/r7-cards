
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Onlyoffice.Api.Logics.Repository;

public interface IFeedRepository
{
    IAsyncEnumerable<FeedDto> GetFiltredAsync(FilterBuilder filterBuilder);
}
