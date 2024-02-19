using System.Net.Http.Json;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;
using TaskStatusDto = Onlyoffice.Api.Models.TaskStatusDto;

namespace Onlyoffice.Api.Logics;

public class ProjectApi(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IProjectApi
{
    #region CRUD Project
    public async Task<ProjectDto> GetProjectByIdAsync(int projectId)
    {
        var project = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<SingleProjectDao>($"api/project/{projectId}"));
        return project?.Response ?? throw new NullReferenceException("Project was not found");
    }

    public async IAsyncEnumerable<ProjectDto> GetProjectsAsync()
    {
        var projectDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectDao>("api/project"));
        await foreach (var project in projectDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<ProjectDto>())
        {
            yield return project;
        }
    }

    public async IAsyncEnumerable<ProjectInfo> GetUserProjectsAsync()
    {
        var projectDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectInfoDao>("api/project/@self"));
        await foreach (var project in projectDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<ProjectInfo>())
        {
            yield return project;
        }
    }

    public async IAsyncEnumerable<UserProfileDto> GetProjectTeamAsync(int projectId)
    {
        var projectTeamDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<UserProfilesDao>($"api/project/{projectId}/team"));
        await foreach (var userProfile in projectTeamDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<UserProfileDto>())
        {
            yield return userProfile;
        }
    }
    #endregion

    #region CRUD Task
    public async IAsyncEnumerable<TaskStatusDto> GetAllTaskStatusesAsync()
    {
        var taskStatusDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskStatusDao>("api/project/status"));
        await foreach (var taskStatus in taskStatusDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TaskStatusDto>())
        {
            yield return taskStatus;
        }
    }

    public async IAsyncEnumerable<TaskDto> GetTasksByProjectIdAsync(int projectId)
    {
        var taskDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskDao>($"api/project/{projectId}/task"));
        await foreach (var task in taskDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<Models.TaskDto>())
        {
            yield return task;
        }
    }

    public async Task<TaskDto> UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}/status", new { status, statusId }));
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not updated");
    }

    public async IAsyncEnumerable<TaskDto> GetFiltredTasksAsync(FilterTasksBuilder builder)
    {
        var filterTasksDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<FilterTasksDao>($"api/project/task/{builder.Build()}"));
        await foreach (var task in filterTasksDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<Models.TaskDto>())
        {
            yield return task;
        }
    }

    public async Task<TaskDto> CreateTaskAsync(int projectId, TaskUpdateData state, Status status, int? statusId = null)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/project/{projectId}/task/status", new { state, status, statusId }));
        if (response.IsSuccessStatusCode)
        {
            var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
            return taskDao?.Response ?? throw new NullReferenceException("Task was not created");
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }

    public async Task<TaskDto> DeleteTaskAsync(int taskId)
    {
        var deletedTask = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleTaskDao>($"api/project/task/{taskId}"));
        return deletedTask?.Response ?? throw new NullReferenceException("Task was not deleted");
    }

    public async Task<TaskDto> UpdateTaskAsync(int taskId, TaskUpdateData state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}", state));
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not updated");
    }
    #endregion

    #region CRUD Subtask
    public async Task<SubtaskDto> UpdateSubtaskAsync(int taskId, int subtaskId, SubtaskUpdateData state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}/{subtaskId}", state));
        var subtaskDao = await response.Content.ReadFromJsonAsync<SingleSubtaskDao>();
        return subtaskDao?.Response ?? throw new NullReferenceException("Subtask was not updated");
    }    

    public async Task<SubtaskDto> DeleteSubtaskAsync(int taskId, int subtaskId)
    {
        var deletedSubtask = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleSubtaskDao>($"api/project/task/{taskId}/{subtaskId}"));
        return deletedSubtask?.Response ?? throw new NullReferenceException("Subtask was not deleted");
    }

    public async Task<SubtaskDto> UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}/{subtaskId}/status", new { status }));

        var subtaskDto = await response.Content.ReadFromJsonAsync<SingleSubtaskDao>();
        return subtaskDto?.Response ?? throw new NullReferenceException("Subtask was not updated");
    }

    public async Task<SubtaskDto> CreateSubtaskAsync(int taskId, string title, string? responsible = null)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/project/task/{taskId}", new { title, responsible }));
        var subtaskDao = await response.Content.ReadFromJsonAsync<SingleSubtaskDao>();
        return subtaskDao?.Response ?? throw new NullReferenceException("Subtask was not created");
    }
    #endregion

    #region CRUD MilestoneDto
    public async IAsyncEnumerable<MilestoneDto> GetMilestonesByProjectIdAsync(int projectId)
    {
        var milestoneDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<MilestoneDao>($"api/project/{projectId}/milestone"));
        await foreach (var milestone in milestoneDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<MilestoneDto>())
        {
            yield return milestone;
        }
    }

    public async Task<MilestoneDto> UpdateMilestoneAsync(int milestoneId, MilestoneUpdateData state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/milestone/{milestoneId}", state));
        var milestoneDao = await response.Content.ReadFromJsonAsync<SingleMilestoneDao>();
        return milestoneDao?.Response ?? throw new NullReferenceException("MilestoneDto was not updated " + response.ReasonPhrase);
    }

    public async Task<MilestoneDto> DeleteMilestoneAsync(int milestoneId)
    {
        var milestoneDao = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleMilestoneDao>($"api/project/milestone/{milestoneId}"));
        return milestoneDao?.Response ?? throw new NullReferenceException("MilestoneDto was not deleted");
    }

    public Task UpdateMilestoneStatusAsync(int milestoneId, Status status)
    {
        return InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/milestone/{milestoneId}/status", new { status }));
    }

    public async Task<MilestoneDto> CreateMilestoneAsync(int projectId, MilestoneUpdateData state)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/project/{projectId}/milestone", state));
        var milestoneDao = await response.Content.ReadFromJsonAsync<SingleMilestoneDao>();
        return milestoneDao?.Response ?? throw new NullReferenceException("MilestoneDto was not created " + response.ReasonPhrase);
    }

    public async Task<TaskDto> GetTaskByIdAsync(int taskId)
    {
        var taskDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<SingleTaskDao>($"api/project/task/{taskId}"));
        return taskDao?.Response ?? throw new NullReferenceException("Task was not found");
    }
    #endregion
}