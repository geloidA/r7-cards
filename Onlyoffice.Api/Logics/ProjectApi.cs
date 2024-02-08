using System.Net.Http.Json;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;
using TaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Onlyoffice.Api.Logics;

public class ProjectApi(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IProjectApi
{
    #region CRUD Project
    public async Task<Project> GetProjectByIdAsync(int projectId)
    {
        var project = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<SingleProjectDao>($"api/project/{projectId}"));
        return project?.Response ?? throw new NullReferenceException("Project was not found");
    }

    public async IAsyncEnumerable<Project> GetProjectsAsync()
    {
        var projectDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectDao>("api/project"));
        await foreach (var project in projectDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<Project>())
        {
            yield return project;
        }
    }

    public async IAsyncEnumerable<Project> GetUserProjectsAsync()
    {
        var projectDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectDao>("api/project/@self"));
        await foreach (var project in projectDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<Project>())
        {
            yield return project;
        }
    }

    public async IAsyncEnumerable<UserProfile> GetProjectTeamAsync(int projectId)
    {
        var projectTeamDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<UserProfilesDao>($"api/project/{projectId}/team"));
        await foreach (var userProfile in projectTeamDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<UserProfile>())
        {
            yield return userProfile;
        }
    }
    #endregion

    #region CRUD Task
    public async IAsyncEnumerable<TaskStatus> GetAllTaskStatusesAsync()
    {
        var taskStatusDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskStatusDao>("api/project/status"));
        await foreach (var taskStatus in taskStatusDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TaskStatus>())
        {
            yield return taskStatus;
        }
    }

    public async IAsyncEnumerable<Models.Task> GetTasksByProjectIdAsync(int projectId)
    {
        var taskDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskDao>($"api/project/{projectId}/task"));
        await foreach (var task in taskDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<Models.Task>())
        {
            yield return task;
        }
    }

    public async System.Threading.Tasks.Task UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null)
    {
        await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}/status", new { status, statusId }));
    }

    public async IAsyncEnumerable<Models.Task> GetFiltredTasksAsync(FilterTasksBuilder builder)
    {
        var filterTasksDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<FilterTasksDao>($"api/project/task/{builder.Build()}"));
        await foreach (var task in filterTasksDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<Models.Task>())
        {
            yield return task;
        }
    }

    public async Task<Models.Task> CreateTaskAsync(int projectId, UpdatedStateTask state, Status status, int? statusId = null)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/project/{projectId}/task/status", new { state, status, statusId }));
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not created");
    }

    public async Task<Models.Task> DeleteTaskAsync(int taskId)
    {
        var deletedTask = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleTaskDao>($"api/project/task/{taskId}"));
        return deletedTask?.Response ?? throw new NullReferenceException("Task was not deleted");
    }

    public async Task<Models.Task> UpdateTaskAsync(int taskId, UpdatedStateTask state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}", state));
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not updated");
    }
    #endregion

    #region CRUD Subtask
    public System.Threading.Tasks.Task UpdateSubtaskAsync(int taskId, int subtaskId, UpdatedStateSubtask state)
    {
        return InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}/{subtaskId}", state));
    }    

    public async Task<Subtask> DeleteSubtaskAsync(int taskId, int subtaskId)
    {
        var deletedSubtask = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleSubtaskDao>($"api/project/task/{taskId}/{subtaskId}"));
        return deletedSubtask?.Response ?? throw new NullReferenceException("Subtask was not deleted");
    }

    public System.Threading.Tasks.Task UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status)
    {
        return InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/task/{taskId}/{subtaskId}/status", new { status }));
    }

    public async Task<Subtask> CreateSubtaskAsync(int taskId, string title, string? responsible = null)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/project/task/{taskId}", new { title, responsible }));
        var subtaskDao = await response.Content.ReadFromJsonAsync<SingleSubtaskDao>();
        return subtaskDao?.Response ?? throw new NullReferenceException("Subtask was not created");
    }
    #endregion

    #region CRUD Milestone
    public async IAsyncEnumerable<Milestone> GetMilestonesByProjectIdAsync(int projectId)
    {
        var milestoneDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<MilestoneDao>($"api/project/{projectId}/milestone"));
        await foreach (var milestone in milestoneDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<Milestone>())
        {
            yield return milestone;
        }
    }

    public async Task<Milestone> UpdateMilestoneAsync(int milestoneId, UpdatedStateMilestone state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/milestone/{milestoneId}", state));
        var milestoneDao = await response.Content.ReadFromJsonAsync<SingleMilestoneDao>();
        return milestoneDao?.Response ?? throw new NullReferenceException("Milestone was not updated " + response.ReasonPhrase);
    }

    public async Task<Milestone> DeleteMilestoneAsync(int milestoneId)
    {
        var milestoneDao = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleMilestoneDao>($"api/project/milestone/{milestoneId}"));
        return milestoneDao?.Response ?? throw new NullReferenceException("Milestone was not deleted");
    }

    public System.Threading.Tasks.Task UpdateMilestoneStatusAsync(int milestoneId, Status status)
    {
        return InvokeHttpClientAsync(c => c.PutAsJsonAsync($"api/project/milestone/{milestoneId}/status", new { status }));
    }

    public async Task<Milestone> CreateMilestoneAsync(int projectId, UpdatedStateMilestone state)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/project/{projectId}/milestone", state));
        var milestoneDao = await response.Content.ReadFromJsonAsync<SingleMilestoneDao>();
        return milestoneDao?.Response ?? throw new NullReferenceException("Milestone was not created " + response.ReasonPhrase);
    }
    #endregion
}