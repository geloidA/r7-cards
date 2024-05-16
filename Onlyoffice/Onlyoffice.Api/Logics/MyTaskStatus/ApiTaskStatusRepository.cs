using System.Net.Http.Json;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api;

public class ApiTaskStatusRepository(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), ITaskStatusRepository
{
    public async IAsyncEnumerable<TaskStatusDto> GetAllAsync()
    {
        var taskStatusDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskStatusDao>($"{ApiPaths.Project}/status"));
        await foreach (var taskStatus in taskStatusDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TaskStatusDto>())
            yield return taskStatus;
    }
}
