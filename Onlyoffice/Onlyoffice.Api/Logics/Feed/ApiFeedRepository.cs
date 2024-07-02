using System.Net.Http.Json;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Onlyoffice.Api.Logics.Feed;

public class ApiFeedRepository(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IFeedRepository
{
    public async IAsyncEnumerable<FeedDto> GetFiltredAsync(FilterBuilder filterBuilder)
    {
        var response = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<FeedDao>($"{ApiPaths.Feed}/filter/{filterBuilder.Build()}"));
        foreach (var feed in response?.Response?.Feeds ?? [])
        {
            yield return feed;
        }
    }
}
