using System.Net.Http.Json;
using Cardmngr.Network.Models;

namespace Cardmngr.Network.Logics;

public class ProjectApi(IHttpClientFactory httpClientFactory) : AuthApiLogic(httpClientFactory), IProjectApi
{
    public Task<Project> GetProjectById(int projectId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Project>> GetProjects()
    {
        var client = httpClientFactory.CreateClient("api");

        var projectDao = await client.GetFromJsonAsync<ProjectDao>("api/2.0/project");

        return projectDao?.Response ?? [];
    }

    public async Task<IEnumerable<Project>> GetUserProjects()
    {
        var client = httpClientFactory.CreateClient("api");

        var projectDao = await client.GetFromJsonAsync<ProjectDao>("api/2.0/project/@self");

        return projectDao?.Response ?? [];
    }
}
