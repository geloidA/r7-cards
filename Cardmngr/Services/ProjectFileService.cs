using System.Net.Http.Json;
using Onlyoffice.Api;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Models;

namespace Cardmngr.Services;

public class ProjectFileService(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IProjectApi
{
    #region Other
    public Task<Milestone> CreateMilestoneAsync(int projectId, UpdatedStateMilestone state)
    {
        throw new NotImplementedException();
    }

    public Task<Subtask> CreateSubtaskAsync(int taskId, string title, string? responsible = null)
    {
        throw new NotImplementedException();
    }

    public Task<Onlyoffice.Api.Models.Task> CreateTaskAsync(int projectId, UpdatedStateTask state, Status status, int? statusId = null)
    {
        throw new NotImplementedException();
    }

    public Task<Milestone> DeleteMilestoneAsync(int milestoneId)
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

    public IAsyncEnumerable<Milestone> GetMilestonesByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Project> GetProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Project> GetUserProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Milestone> UpdateMilestoneAsync(int milestoneId, UpdatedStateMilestone state)
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

    public Task<Onlyoffice.Api.Models.Task> UpdateTaskAsync(int taskId, UpdatedStateTask state)
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<UserProfile> GetProjectTeamAsync(int projectId)
    {
        throw new NotImplementedException();
    }
    #endregion

    public Task<Onlyoffice.Api.Models.Task> DeleteTaskAsync(int taskId)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Onlyoffice.Api.Models.TaskStatus> GetAllTaskStatusesAsync()
    {
        return GetItemsFrom<Onlyoffice.Api.Models.TaskStatus>("api/projectFileApi/all-statuses");
    }

    public IAsyncEnumerable<Onlyoffice.Api.Models.Task> GetTasksByProjectIdAsync(int projectId)
    {
        return GetItemsFrom<Onlyoffice.Api.Models.Task>("api/projectFileApi/all-tasks");
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
