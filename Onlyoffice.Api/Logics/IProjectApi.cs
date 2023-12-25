using Onlyoffice.Api.Models;
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
}

public static class ProjectApiExtensions
{
    public static async Task<List<List<MyTask>>> GetTasksForProjectsAsync(this IProjectApi api, IEnumerable<int> projectIds)
    {
        var tasks = projectIds.Select(api.GetTasksByProjectIdAsync);

        var taskResults = await Task.WhenAll(tasks);

        return [.. taskResults];
    }
}
