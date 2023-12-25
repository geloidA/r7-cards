using System.Net.Http.Json;
using Onlyoffice.Api.Models;
using TaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Onlyoffice.Api.Logics;

public class ProjectApi(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IProjectApi
{
    public Task<Project> GetProjectByIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        var client = httpClientFactory.CreateClient("api");

        var projectDao = await client.GetFromJsonAsync<ProjectDao>("api/project");

        return projectDao?.Response ?? [];
    }

    public async Task<List<Project>> GetUserProjectsAsync()
    {
        var client = httpClientFactory.CreateClient("api");

        var projectDao = await client.GetFromJsonAsync<ProjectDao>("api/project/@self");

        return projectDao?.Response ?? [];
    }

    public async Task<List<TaskStatus>> GetAllTaskStatusesAsync()
    {
        var client = httpClientFactory.CreateClient("api");

        var taskStatusDao = await client.GetFromJsonAsync<TaskStatusDao>("api/project/status");

        return taskStatusDao?.Response ?? [];
    }

    public async Task<List<Models.Task>> GetTasksByProjectIdAsync(int projectId)
    {
        var client = httpClientFactory.CreateClient("api");

        var taskDao = await client.GetFromJsonAsync<TaskDao>($"api/project/{projectId}/task");

        return taskDao?.Response ?? [];
    }
}
