using System.Net.Http.Json;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Group;

public class GroupApi(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IGroupApi
{
    public async IAsyncEnumerable<GroupDto> GetEntitiesAsync() // TODO: DRY
    {
        var groupsDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<GroupsDao>(ApiPaths.Groups));
        await foreach (var group in groupsDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<GroupDto>())
        {
            yield return group;
        }
    }

    public IAsyncEnumerable<GroupDto> GetEntitiesAsync(FilterBuilder filterBuilder)
    {
        throw new NotImplementedException();
    }
}
