using Cardmngr.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Onlyoffice.Api.Models;

namespace Cardmngr.Server;

[ApiController]
[Route("api/[controller]")]
public class ProjectFileApiController(IProjectFileService projectFile) : Controller
{
    private readonly IProjectFileService projectFile = projectFile;

    [HttpGet("project")]
    public Task<Project> GetProject() => projectFile.GetProject();

    [HttpGet("all-statuses")]
    public IAsyncEnumerable<Onlyoffice.Api.Models.TaskStatus> GetTaskStatuses() => projectFile.GetTaskStatuses();

    [HttpGet("all-tasks")]
    public IAsyncEnumerable<Onlyoffice.Api.Models.Task> GetTasksAsync() => projectFile.GetTasksAsync();
}
