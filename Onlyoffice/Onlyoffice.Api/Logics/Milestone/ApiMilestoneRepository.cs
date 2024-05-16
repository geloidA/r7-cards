using System.Net.Http.Json;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Milestone;

public class ApiMilestoneRepository(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IMilestoneRepository
{
    public async Task<MilestoneDto> CreateAsync(int projectId, MilestoneUpdateData state)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"{ApiPaths.Project}/{projectId}/milestone", state));
        var milestoneDao = await response.Content.ReadFromJsonAsync<SingleMilestoneDao>();
        return milestoneDao?.Response ?? throw new NullReferenceException("MilestoneDto was not created " + response.ReasonPhrase);
    }

    public async Task<MilestoneDto> DeleteAsync(int id)
    {
        var milestoneDao = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleMilestoneDao>($"{ApiPaths.Milestone}/{id}"));
        return milestoneDao?.Response ?? throw new NullReferenceException("MilestoneDto was not deleted");
    }

    public async IAsyncEnumerable<MilestoneDto> GetAllByProjectIdAsync(int projectId)
    {
        var milestoneDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<MilestoneDao>($"{ApiPaths.Project}/{projectId}/milestone"));
        await foreach (var milestone in milestoneDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<MilestoneDto>())
            yield return milestone;
    }

    public async Task<MilestoneDto> GetByIdAsync(int id)
    {
        var milestoneDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<SingleMilestoneDao>($"{ApiPaths.Milestone}/{id}"));
        return milestoneDao?.Response ?? throw new NullReferenceException("MilestoneDto was not found");
    }

    public async Task<MilestoneDto> UpdateAsync(int id, MilestoneUpdateData state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"{ApiPaths.Milestone}/{id}", state));
        var milestoneDao = await response.Content.ReadFromJsonAsync<SingleMilestoneDao>();
        return milestoneDao?.Response ?? throw new NullReferenceException("MilestoneDto was not updated " + response.ReasonPhrase);
    }

    public Task UpdateStatusAsync(int id, Status status)
    {
        return InvokeHttpClientAsync(c => c.PutAsJsonAsync($"{ApiPaths.Milestone}/{id}/status", new { status }));
    }
}
