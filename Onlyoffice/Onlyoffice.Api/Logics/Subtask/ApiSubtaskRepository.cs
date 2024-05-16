using System.Net.Http.Json;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Subtask;

public class ApiSubtaskRepository(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), ISubtaskRepository
{
    public async Task<SubtaskDto> CreateAsync(int taskId, string title, string? responsible = null)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"{ApiPaths.Task}/{taskId}", new { title, responsible }));
        var subtaskDao = await response.Content.ReadFromJsonAsync<SingleSubtaskDao>();
        return subtaskDao?.Response ?? throw new NullReferenceException("Subtask was not created");
    }

    public async Task<SubtaskDto> DeleteAsync(int taskId, int id)
    {
        var deletedSubtask = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleSubtaskDao>($"{ApiPaths.Task}/{taskId}/{id}"));
        return deletedSubtask?.Response ?? throw new NullReferenceException("Subtask was not deleted");
    }

    public async Task<SubtaskDto> UpdateAsync(int taskId, int id, SubtaskUpdateData state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"{ApiPaths.Task}/{taskId}/{id}", state));
        var subtaskDao = await response.Content.ReadFromJsonAsync<SingleSubtaskDao>();
        return subtaskDao?.Response ?? throw new NullReferenceException("Subtask was not updated");
    }

    public async Task<SubtaskDto> UpdateStatusAsync(int taskId, int id, Status status)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"{ApiPaths.Task}/{taskId}/{id}/status", new { status }));

        var subtaskDto = await response.Content.ReadFromJsonAsync<SingleSubtaskDao>();
        return subtaskDto?.Response ?? throw new NullReferenceException("Subtask was not updated");
    }
}
