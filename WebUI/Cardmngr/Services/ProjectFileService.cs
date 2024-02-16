using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Providers;

namespace Cardmngr.Services;

public class ProjectFileService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider provider) : ApiLogicBase(httpClientFactory), IProjectApi
{
    private readonly CookieStateProvider provider = provider.ToCookieProvider();

    #region Other
    public Task<MilestoneDto> CreateMilestoneAsync(int projectId, UpdatedStateMilestone state)
    {
        throw new NotImplementedException();
    }

    public Task<Subtask> CreateSubtaskAsync(int taskId, string title, string? responsible = null)
    {
        throw new NotImplementedException();
    }

    public Task<MilestoneDto> DeleteMilestoneAsync(int milestoneId)
    {
        throw new NotImplementedException();
    }

    public Task<Subtask> DeleteSubtaskAsync(int taskId, int subtaskId)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Onlyoffice.Api.Models.Task> GetFiltredTasksAsync(FilterTasksBuilder builder)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<MilestoneDto> GetMilestonesByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Project> GetProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ProjectInfo> GetUserProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<MilestoneDto> UpdateMilestoneAsync(int milestoneId, UpdatedStateMilestone state)
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task UpdateMilestoneStatusAsync(int milestoneId, Status status)
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task UpdateSubtaskAsync(int taskId, int subtaskId, UpdatedStateSubtask state)
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<UserProfileDto> GetProjectTeamAsync(int projectId)
    {
        throw new NotImplementedException();
    }
    #endregion

    public async Task<Onlyoffice.Api.Models.Task> CreateTaskAsync(int projectId, UpdatedStateTask state, Status status, int? statusId = null)
    {
        var guid = provider["UserId"];
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/projectFileApi/create-task/{guid}", state), "self-api");
        var task = await response.Content.ReadFromJsonAsync<Onlyoffice.Api.Models.Task>();
        return task ?? throw new NullReferenceException("Task is null");
    }

    public async Task<Onlyoffice.Api.Models.Task> UpdateTaskAsync(int taskId, UpdatedStateTask state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/projectFileApi/update-task/{taskId}", state), "self-api");
        var task = await response.Content.ReadFromJsonAsync<Onlyoffice.Api.Models.Task>();
        return task ?? throw new NullReferenceException("Task is null");
    }

    public async Task<Onlyoffice.Api.Models.Task> DeleteTaskAsync(int taskId)
    {
        var task = await InvokeHttpClientAsync(c => 
            c.DeleteFromJsonAsync<Onlyoffice.Api.Models.Task>($"api/projectFileApi/delete-task/{taskId}"), "self-api");
        return task ?? throw new NullReferenceException("Task is null");
    }

    public async System.Threading.Tasks.Task UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null)
    {
        await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/projectFileApi/update-task-status/{taskId}", statusId), "self-api");
    }

    public IAsyncEnumerable<Onlyoffice.Api.Models.TaskStatus> GetAllTaskStatusesAsync()
    {
        return GetItemsFrom<Onlyoffice.Api.Models.TaskStatus>("api/projectFileApi/all-statuses");
    }

    public IAsyncEnumerable<Onlyoffice.Api.Models.Task> GetTasksByProjectIdAsync(int projectId)
    {
        var guid = provider["UserId"];
        return GetItemsFrom<Onlyoffice.Api.Models.Task>($"api/projectFileApi/all-tasks/{guid}");
    }

    public async Task<Project> GetProjectByIdAsync(int projectId)
    {
        return await InvokeHttpClientAsync(c => c.GetFromJsonAsync<Project>("api/projectFileApi/project"), "self-api") ?? 
            throw new NullReferenceException("Project is null");
    }

    private async IAsyncEnumerable<T> GetItemsFrom<T>(string api)
    {
        var items = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<IAsyncEnumerable<T>>(api), "self-api");

        await foreach (var item in items ?? AsyncEnumerable.Empty<T>())
        {
            yield return item;
        }
    }
}
