using Onlyoffice.Api.Models;
using Onlyoffice.Api.Common;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;
using MyTask = Onlyoffice.Api.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace Onlyoffice.Api.Logics;

public interface IProjectApi
{
    /// <summary>
    /// Returns a list of all the projects in which the current user participates.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/@self"/>
    /// </remarks>
    /// <returns>Projects that belongs to current user</returns>
    Task<List<Project>> GetUserProjectsAsync();

    /// <summary>
    /// Returns a list of all the portal projects with the base information about them.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project"/> 
    /// </remarks>
    /// <returns>List of projects</returns>
    Task<List<Project>> GetProjectsAsync();

    /// <summary>
    /// Returns the detailed information about a project with the specified ID.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/%7bid%7d"/>
    /// </remarks>
    /// <param name="projectId">Project ID</param>
    /// <returns>Project</returns>
    Task<Project> GetProjectByIdAsync(int projectId);

    /// <summary>
    /// Returns all the task statuses.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/status"/>
    /// </remarks>
    /// <returns>Task statuses</returns>
    Task<List<MyTaskStatus>> GetAllTaskStatusesAsync();

    /// <summary>
    /// Returns a list of all the tasks from a project with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/%7bprojectid%7d/task"/>
    /// </remarks>
    /// <param name="projectId">Project ID</param>
    /// <returns>List of tasks</returns>
    Task<List<MyTask>> GetTasksByProjectIdAsync(int projectId);

    /// <summary>
    /// Returns a list with the detailed information about all the tasks matching the parameters specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/task/filter"/>
    /// </remarks>
    /// <param name="builder">Builder for parameters in request</param>
    /// <returns>List of tasks</returns>
    Task<List<MyTask>> GetFiltredTasksAsync(FilterTasksBuilder builder);

    /// <summary>
    /// Adds a task to the selected project with the title.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/post/api/2.0/project/%7bprojectid%7d/task"/>
    /// </remarks>
    /// <param name="projectId">Project ID</param>
    /// <param name="title">Task title</param>
    /// <returns></returns>
    Task<MyTask> CreateTaskAsync(int projectId, string title);

    /// <summary>
    /// Returns a list of all the users participating in the project with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/%7bprojectid%7d/team"/>
    /// </remarks>
    /// <param name="projectId">Project ID</param>
    /// <returns>List of team members</returns>
    Task<List<UserProfile>> GetProjectTeamAsync(int projectId);

    /// <summary>
    /// Deletes a task with the ID specified in the request from the project.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/delete/api/2.0/project/task/%7btaskid%7d"/>
    /// </remarks>
    /// <param name="taskId"></param>
    /// <returns>Deleted task</returns>
    Task<MyTask> DeleteTaskAsync(int taskId);

    /// <summary>
    /// Adds a task to the selected project with the title and status.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/post/api/2.0/project/%7bprojectid%7d/task"/>
    /// </remarks>
    /// <param name="projectId">Project ID</param>
    /// <param name="title">Task title</param>
    /// <param name="status">New task status</param>
    /// <param name="statusId">Custom status ID</param>
    /// <returns>Added task</returns>
    Task<MyTask> CreateTaskAsync(int projectId, string title, Status status, int? statusId = null);

    /// <summary>
    /// Updates the selected task.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/task/%7btaskid%7d"/>
    /// </remarks>
    /// <param name="taskId">Task ID</param>
    /// <param name="state">Updated state</param>
    Task UpdateTaskAsync(int taskId, UpdatedStateTask state);

    /// <summary>
    /// Updates a status of a task with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/task/%7btaskid%7d/status"/>
    /// </remarks>
    /// <param name="taskId">Task ID</param>
    /// <param name="status">New task status</param>
    /// <param name="statusId">Custom status ID</param>
    Task UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null);

    /// <summary>
    /// Updates the selected subtask with the title and responsible specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/task/%7btaskid%7d/%7bsubtaskid%7d"/>
    /// </remarks>
    /// <param name="taskId">Task ID</param>
    /// <param name="subtaskId">Subtask ID</param>
    /// <param name="state">Updated state</param>
    Task UpdateSubtaskAsync(int taskId, int subtaskId, UpdatedStateSubtask state);

    /// <summary>
    /// Deletes the selected subtask from the parent task with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/delete/api/2.0/project/task/%7btaskid%7d/%7bsubtaskid%7d"/>
    /// </remarks>
    /// <param name="taskId">Task ID</param>
    /// <param name="subtaskId">Subtask ID</param>
    /// <returns>Subtask</returns>
    Task<Subtask> DeleteSubtaskAsync(int taskId, int subtaskId);

    /// <summary>
    /// Updates the selected subtask status of the parent task with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/task/%7btaskid%7d/%7bsubtaskid%7d/status"/>
    /// </remarks>
    /// <param name="taskId">Task ID</param>
    /// <param name="subtaskId">Subtask ID</param>
    /// <param name="status">New subtask status</param>
    /// <returns></returns>
    Task UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status);

    /// <summary>
    /// Creates a subtask with the title and responsible within the parent task specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/post/api/2.0/project/task/%7btaskid%7d"/>
    /// </remarks>
    /// <param name="taskId">Task ID</param>
    /// <param name="title">Subtask title</param>
    /// <param name="responsible">Subtask responsible</param>
    /// <returns>Subtask</returns>
    Task<Subtask> CreateSubtaskAsync(int taskId, string title, string? responsible = null);

    /// <summary>
    /// Returns a list of all the milestones from a project with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/%7bid%7d/milestone"/>
    /// </remarks>
    /// <param name="projectId">Project ID</param>
    /// <returns>List of milestones</returns>
    Task<List<Milestone>> GetMilestonesByProjectIdAsync(int projectId);

    /// <summary>
    /// Updates the selected milestone changing the milestone updated state.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/milestone/%7bid%7d"/>
    /// </remarks>
    /// <param name="milestoneId">Milestone ID</param>
    /// <param name="state">Updated state</param>
    Task UpdateMilestoneAsync(int milestoneId, UpdatedStateMilestone state);

    /// <summary>
    /// Deletes a milestone with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/delete/api/2.0/project/milestone/%7bid%7d"/>
    /// </remarks>
    /// <param name="milestoneId"></param>
    /// <returns>Deleted milestone</returns>
    Task<Milestone> DeleteMilestoneAsync(int milestoneId);

    /// <summary>
    /// Updates a status of a milestone with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/milestone/%7bid%7d/status"/>
    /// </remarks>
    /// <param name="milestoneId">Milestone ID</param>
    /// <param name="status">New milestone status</param>
    /// <returns>Updated milestone</returns>
    Task UpdateMilestoneStatusAsync(int milestoneId, CommonStatus status);
}

public static class ProjectApiExtensions
{
    /// <summary>
    /// Get tasks for projects
    /// </summary>
    /// <remarks>
    /// Invokes <see cref="IProjectApi.GetFiltredTasksAsync"/> for each project
    /// </remarks>
    /// <param name="projectIds">Project IDs</param>
    /// <returns>List of list of tasks</returns>
    public static async Task<List<List<MyTask>>> GetTasksForProjectsAsync(this IProjectApi api, params int[] projectIds)
    {
        var tasks = projectIds.Select(x => api.GetFiltredTasksAsync(FilterTasksBuilder.Instance.WithProjectId(x)));

        var taskResults = await Task.WhenAll(tasks);

        return [.. taskResults];
    }
}
