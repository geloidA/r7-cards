using Onlyoffice.Api.Models;
using Onlyoffice.Api.Common;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;
using MyTask = Onlyoffice.Api.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace Onlyoffice.Api.Logics;

public interface IProjectApi
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Projects that belongs to current user</returns>
    Task<List<Project>> GetUserProjectsAsync();
    /// <summary>
    /// 
    /// </summary>
    /// <returns>All projects</returns>
    Task<List<Project>> GetProjectsAsync();
    Task<Project> GetProjectByIdAsync(int projectId);
    Task<List<MyTaskStatus>> GetAllTaskStatusesAsync();
    Task<List<MyTask>> GetTasksByProjectIdAsync(int projectId);
    Task<List<MyTask>> GetFiltredTasksAsync(FilterTasksBuilder builder);
    Task<MyTask> CreateTaskAsync(int projectId, string title);
    Task<List<UserProfile>> GetProjectTeam(int projectId);
    Task<MyTask> DeleteTaskAsync(int taskId);
    Task<MyTask> CreateTaskAsync(int projectId, string title, Status status, int? statusId = null);
    Task UpdateTaskAsync(int taskId, UpdatedStateTask state);
    Task UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null);
    Task UpdateSubtaskAsync(int taskId, int subtaskId, UpdatedStateSubtask state);
    Task<Subtask> DeleteSubtaskAsync(int taskId, int subtaskId);
    Task UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status);
    Task<Subtask> CreateSubtaskAsync(int taskId, string title, string? responsible = null);
}

public static class ProjectApiExtensions
{
    public static async Task<List<List<MyTask>>> GetTasksForProjectsAsync(this IProjectApi api, IEnumerable<int> projectIds)
    {
        var tasks = projectIds.Select(x => api.GetFiltredTasksAsync(FilterTasksBuilder.Instance.WithProjectId(x)));

        var taskResults = await Task.WhenAll(tasks);

        return [.. taskResults];
    }
}
