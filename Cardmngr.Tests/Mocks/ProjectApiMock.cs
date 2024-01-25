using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Models;

namespace Cardmngr.Tests;

public class ProjectApiMock : IProjectApi
{
    public Task<Subtask> CreateSubtaskAsync(int taskId, string title, string? responsible = null)
    {
        throw new NotImplementedException();
    }

    public Task<Onlyoffice.Api.Models.Task> CreateTaskAsync(int projectId, string title)
    {
        throw new NotImplementedException();
    }

    public Task<Onlyoffice.Api.Models.Task> CreateTaskAsync(int projectId, string title, Status status, int? statusId = null)
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

    public Task<Onlyoffice.Api.Models.Task> DeleteTaskAsync(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Onlyoffice.Api.Models.TaskStatus>> GetAllTaskStatusesAsync()
    {
        return System.Threading.Tasks.Task.FromResult<List<Onlyoffice.Api.Models.TaskStatus>>([
            new Onlyoffice.Api.Models.TaskStatus { Id = 1, StatusType = 0 },
            new Onlyoffice.Api.Models.TaskStatus { Id = 2, StatusType = 1 },
            new Onlyoffice.Api.Models.TaskStatus { Id = 3, StatusType = 2 }
        ]);
    }

    public Task<List<Onlyoffice.Api.Models.Task>> GetFiltredTasksAsync(FilterTasksBuilder builder)
    {
        return System.Threading.Tasks.Task.FromResult<List<Onlyoffice.Api.Models.Task>>([
            new Onlyoffice.Api.Models.Task { Id = 1, StartDate = DateTime.Now },
            new Onlyoffice.Api.Models.Task { Id = 2, StartDate = DateTime.Now }
        ]);
    }

    public Task<List<Milestone>> GetMilestonesByProjectIdAsync(int projectId)
    {
        return System.Threading.Tasks.Task.FromResult<List<Milestone>>([
            new Milestone { Id = 1, Title = "test", Deadline = DateTime.Now },
            new Milestone { Id = 2, Title = "test2", Deadline = DateTime.Now }
        ]);
    }

    public Task<Project> GetProjectByIdAsync(int projectId)
    {
        return System.Threading.Tasks.Task.FromResult(new Project
        {
            Id = projectId
        });
    }

    public Task<List<Project>> GetProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<UserProfile>> GetProjectTeamAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Onlyoffice.Api.Models.Task>> GetTasksByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Project>> GetUserProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task UpdateMilestoneAsync(int milestoneId, UpdatedStateMilestone state)
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

    public System.Threading.Tasks.Task UpdateTaskAsync(int taskId, UpdatedStateTask state)
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null)
    {
        throw new NotImplementedException();
    }

    Task<Onlyoffice.Api.Models.Task> IProjectApi.UpdateTaskAsync(int taskId, UpdatedStateTask state)
    {
        throw new NotImplementedException();
    }
}
