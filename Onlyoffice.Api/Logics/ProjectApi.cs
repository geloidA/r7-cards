using System.Net.Http.Json;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;
using TaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Onlyoffice.Api.Logics;

public class ProjectApi(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IProjectApi
{
    public async Task<Project> GetProjectByIdAsync(int projectId)
    {
        var project = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<SingleProjectDao>($"api/project/{projectId}"));
        return project?.Response ?? throw new NullReferenceException("Project was not found");
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        var projectDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectDao>("api/project"));
        return projectDao?.Response ?? [];
    }

    public async Task<List<Project>> GetUserProjectsAsync()
    {
        var projectDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectDao>("api/project/@self"));
        return projectDao?.Response ?? [];
    }

    public async Task<List<TaskStatus>> GetAllTaskStatusesAsync()
    {
        var taskStatusDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskStatusDao>("api/project/status"));
        return taskStatusDao?.Response ?? [];
    }

    public async Task<List<Models.Task>> GetTasksByProjectIdAsync(int projectId)
    {
        var taskDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskDao>($"api/project/{projectId}/task"));
        return taskDao?.Response ?? [];
    }

    public async System.Threading.Tasks.Task UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null)
    {
        await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}/status", new { status, statusId }));
    }

    public async Task<List<Models.Task>> GetFiltredTasksAsync(FilterTasksBuilder builder)
    {
        var filterTasksDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<FilterTasksDao>($"api/project/task/{builder.Build()}"));
        return filterTasksDao?.Response ?? [];
    }

    public async Task<Models.Task> CreateTaskAsync(int projectId, string title)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/project/{projectId}/task", new { title }));
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not created");
    }

    public async Task<Models.Task> CreateTaskAsync(int projectId, string title, Status status, int? statusId = null)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/project/{projectId}/task/status", new { title, status, statusId }));
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not created");
    }

    private async Task<T> InvokeHttpClientAsync<T>(Func<HttpClient, Task<T>> func)
    {
        using var client = httpClientFactory.CreateClient("api");
        return await func(client);
    }

    private async System.Threading.Tasks.Task InvokeHttpClientAsync<T>(Func<HttpClient, System.Threading.Tasks.Task> func)
    {
        using var client = httpClientFactory.CreateClient("api");
        await func(client);
    }
}
