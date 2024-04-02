using Onlyoffice.Api.Models;
using Onlyoffice.Api.Common;

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
    IAsyncEnumerable<ProjectInfoDto> GetUserProjectsAsync();

    /// <summary>
    /// Returns a list of all the portal projects with the base information about them.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project"/> 
    /// </remarks>
    /// <returns>List of projects</returns>
    IAsyncEnumerable<ProjectDto> GetProjectsAsync();

    /// <summary>
    /// Returns the detailed information about a project with the specified ID.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/%7bid%7d"/>
    /// </remarks>
    /// <param name="projectId">ProjectDto ID</param>
    /// <returns>ProjectDto</returns>
    Task<ProjectDto> GetProjectByIdAsync(int projectId);

    /// <summary>
    /// Returns all the task statuses.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/status"/>
    /// </remarks>
    /// <returns>TaskDto statuses</returns>
    IAsyncEnumerable<TaskStatusDto> GetAllTaskStatusesAsync();

    /// <summary>
    /// Returns a list of all the tasks from a project with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/%7bprojectid%7d/task"/>
    /// </remarks>
    /// <param name="projectId">ProjectDto ID</param>
    /// <returns>List of tasks</returns>
    IAsyncEnumerable<TaskDto> GetTasksByProjectIdAsync(int projectId);

    /// <summary>
    /// Returns the detailed information about a task with the specified ID.
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    Task<TaskDto> GetTaskByIdAsync(int taskId);

    /// <summary>
    /// Returns a list with the detailed information about all the tasks matching the parameters specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/task/filter"/>
    /// </remarks>
    /// <param name="builder">Builder for parameters in request</param>
    /// <returns>List of tasks</returns>
    IAsyncEnumerable<TaskDto> GetFiltredTasksAsync(FilterBuilder builder);

    /// <summary>
    /// Returns a list of all the users participating in the project with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/%7bprojectid%7d/team"/>
    /// </remarks>
    /// <param name="projectId">ProjectDto ID</param>
    /// <returns>List of team members</returns>
    IAsyncEnumerable<UserProfileDto> GetProjectTeamAsync(int projectId);

    /// <summary>
    /// Deletes a task with the ID specified in the request from the project.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/delete/api/2.0/project/task/%7btaskid%7d"/>
    /// </remarks>
    /// <param name="taskId"></param>
    /// <returns>Deleted task</returns>
    Task<TaskDto> DeleteTaskAsync(int taskId);

    /// <summary>
    /// Adds a task to the selected project with the title and status.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/post/api/2.0/project/%7bprojectid%7d/task"/>
    /// </remarks>
    /// <param name="projectId">ProjectDto ID</param>
    /// <param name="state">responsible user ID, task description, deadline time, etc</param>
    /// <param name="status">New task status</param>
    /// <param name="statusId">Custom status ID</param>
    /// <returns>Added task</returns>
    Task<TaskDto> CreateTaskAsync(int projectId, TaskUpdateData state, Status status, int? statusId = null);

    /// <summary>
    /// Returns a list with the detailed information about all the tasks for the current user.
    /// </summary>
    /// <remarks>
    /// <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/task/%40self"/> 
    /// </remarks>
    /// <returns>List of tasks</returns>
    IAsyncEnumerable<TaskDto> GetSelfTasksAsync();

    /// <summary>
    /// Updates the selected task.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/task/%7btaskid%7d"/>
    /// </remarks>
    /// <param name="taskId">TaskDto ID</param>
    /// <param name="state">Updated state</param>
    Task<TaskDto> UpdateTaskAsync(int taskId, TaskUpdateData state);

    /// <summary>
    /// Updates a status of a task with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/task/%7btaskid%7d/status"/>
    /// </remarks>
    /// <param name="taskId">TaskDto ID</param>
    /// <param name="status">New task status</param>
    /// <param name="statusId">Custom status ID</param>
    Task<TaskDto> UpdateTaskStatusAsync(int taskId, Status status, int? statusId = null);

    /// <summary>
    /// Updates the selected subtask with the title and responsible specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/task/%7btaskid%7d/%7bsubtaskid%7d"/>
    /// </remarks>
    /// <param name="taskId">TaskDto ID</param>
    /// <param name="subtaskId">SubtaskDto ID</param>
    /// <param name="state">Updated state</param>
    Task<SubtaskDto> UpdateSubtaskAsync(int taskId, int subtaskId, SubtaskUpdateData state);

    /// <summary>
    /// Deletes the selected subtask from the parent task with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/delete/api/2.0/project/task/%7btaskid%7d/%7bsubtaskid%7d"/>
    /// </remarks>
    /// <param name="taskId">TaskDto ID</param>
    /// <param name="subtaskId">SubtaskDto ID</param>
    /// <returns>SubtaskDto</returns>
    Task<SubtaskDto> DeleteSubtaskAsync(int taskId, int subtaskId);

    /// <summary>
    /// Updates the selected subtask status of the parent task with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/task/%7btaskid%7d/%7bsubtaskid%7d/status"/>
    /// </remarks>
    /// <param name="taskId">TaskDto ID</param>
    /// <param name="subtaskId">SubtaskDto ID</param>
    /// <param name="status">New subtask status</param>
    /// <returns></returns>
    Task<SubtaskDto> UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status);

    /// <summary>
    /// Creates a subtask with the title and responsible within the parent task specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/post/api/2.0/project/task/%7btaskid%7d"/>
    /// </remarks>
    /// <param name="taskId">TaskDto ID</param>
    /// <param name="title">SubtaskDto title</param>
    /// <param name="responsible">SubtaskDto responsible</param>
    /// <returns>SubtaskDto</returns>
    Task<SubtaskDto> CreateSubtaskAsync(int taskId, string title, string? responsible = null);

    /// <summary>
    /// Returns a list of all the milestones from a project with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/%7bid%7d/milestone"/>
    /// </remarks>
    /// <param name="projectId">ProjectDto ID</param>
    /// <returns>List of milestones</returns>
    IAsyncEnumerable<MilestoneDto> GetMilestonesByProjectIdAsync(int projectId);

    /// <summary>
    /// Updates the selected milestone changing the milestone updated state.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/milestone/%7bid%7d"/>
    /// </remarks>
    /// <param name="milestoneId">MilestoneDto ID</param>
    /// <param name="state">Updated state</param>
    Task<MilestoneDto> UpdateMilestoneAsync(int milestoneId, MilestoneUpdateData state);

    /// <summary>
    /// Adds a new milestone using the parameters (project ID, milestone title, deadline, etc) specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/post/api/2.0/project/%7bid%7d/milestone"/>
    /// </remarks>
    /// <param name="projectId"></param>
    /// <param name="state"></param>
    /// <returns>Added milestone</returns>
    Task<MilestoneDto> CreateMilestoneAsync(int projectId, MilestoneUpdateData state);

    /// <summary>
    /// Deletes a milestone with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/delete/api/2.0/project/milestone/%7bid%7d"/>
    /// </remarks>
    /// <param name="milestoneId"></param>
    /// <returns>Deleted milestone</returns>
    Task<MilestoneDto> DeleteMilestoneAsync(int milestoneId);

    /// <summary>
    /// Updates a status of a milestone with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/put/api/2.0/project/milestone/%7bid%7d/status"/>
    /// </remarks>
    /// <param name="milestoneId">MilestoneDto ID</param>
    /// <param name="status">New milestone status</param>
    /// <returns>Updated milestone</returns>
    Task UpdateMilestoneStatusAsync(int milestoneId, Status status);

    /// <summary>
    /// Returns the detailed information about a milestone with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/milestone/%7bid%7d"/>
    /// </remarks>
    /// <param name="milestoneId"></param>
    /// <returns>Milestone</returns>
    Task<MilestoneDto> GetMilestoneByIdAsync(int milestoneId);

    /// <summary>
    /// Returns a list of the comments for the task with the ID specified in the request.
    /// </summary>
    /// <remarks>
    /// Api doc: <see cref="https://api.onlyoffice.com/portals/method/project/get/api/2.0/project/task/%7btaskid%7d/comment"/>
    /// </remarks>
    /// <param name="taskId"></param>
    /// <returns>Comments</returns>
    IAsyncEnumerable<CommentDto> GetTaskCommentsAsync(int taskId);

    Task<CommentDto> CreateTaskCommentAsync(int taskId, CommentUpdateData comment);

    Task RemoveTaskCommentAsync(string commentId);
}
