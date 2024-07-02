using System.Net.Http.Json;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Onlyoffice.Api.Logics.Group;

public class ApiGroupRepository(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IGroupRepository
{
    public async IAsyncEnumerable<GroupDto> GetAllAsync() // TODO: DRY
    {
        var groupsDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<GroupsDao>(ApiPaths.Group));
        await foreach (var group in groupsDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<GroupDto>())
        {
            yield return group;
        }
    }

    public IAsyncEnumerable<GroupDto> GetFiltredAsync(FilterBuilder filterBuilder)
    {
        throw new NotImplementedException();
    }
}
