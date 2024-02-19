using Cardmngr.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Onlyoffice.Api.Models;

using Task = System.Threading.Tasks.Task;

namespace Cardmngr.Server;

[ApiController]
[Route("api/[controller]")]
public class ProjectFileApiController(IProjectFileService projectFile) : Controller
{
    private readonly IProjectFileService projectFile = projectFile;

    [HttpGet("project")]
    public Task<ProjectDto> GetProject() => projectFile.GetProject();

    [HttpGet("all-statuses")]
    public IAsyncEnumerable<Onlyoffice.Api.Models.TaskStatusDto> GetTaskStatuses() => projectFile.GetTaskStatuses();

    [HttpGet("all-tasks/{guid}")]
    public IAsyncEnumerable<Onlyoffice.Api.Models.TaskDto> GetTasksAsync(string guid) => projectFile.GetTasksAsync(guid);

    [HttpPost("create-task/{guid}")]
    public async Task<IActionResult> CreateTask(string guid, TaskUpdateData state)
    {
        var created = await projectFile.CreateTask(guid, state);
        return Ok(created);
    }

    [HttpDelete("delete-task/{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var deleted = await projectFile.DeleteTaskAsync(taskId);
        return Ok(deleted);
    }

    [HttpPut("update-task/{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, TaskUpdateData state)
    {
        var updated = await projectFile.UpdateTask(taskId, state);
        return Ok(updated);
    }

    [HttpPut("update-task-status/{taskId}")]
    public Task UpdateTaskStatus(int taskId, [FromBody] int statusId) => projectFile.UpdateTaskStatus(taskId, statusId);
}
