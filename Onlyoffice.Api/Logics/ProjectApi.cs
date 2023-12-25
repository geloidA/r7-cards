using System.Net.Http.Json;
using Onlyoffice.Api.Common;
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

    public async System.Threading.Tasks.Task UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null)
    {
        var client = httpClientFactory.CreateClient("api");
        await client.PutAsJsonAsync($"api/project/task/{taskId}/status", new { status, statusId });
    }

    public async Task<List<Models.Task>> GetFiltredTasksAsync(FilterTasksBuilder builder)
    {
        var client = httpClientFactory.CreateClient("api");
        var filterTasksDao = await client.GetFromJsonAsync<FilterTasksDao>($"api/project/task/{builder.Build()}");
        return filterTasksDao?.Response ?? [];
    }

    public async Task<Models.Task> CreateTaskAsync(int projectId, string title)
    {
        var client = httpClientFactory.CreateClient("api");
        var response = await client.PostAsJsonAsync($"api/project/{projectId}/task", new { title });
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not created");
    }
}
