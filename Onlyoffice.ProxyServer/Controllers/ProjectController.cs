using Microsoft.AspNetCore.Mvc;
namespace Onlyoffice.ProxyServer.Controllers;

public class ProjectController(IConfiguration conf) : ApiController(conf)
{
    [Route("api/[controller]")]
    public Task ProxyProject() => ProxyRequestAsync($"{apiUrl}/project");

    [Route("api/[controller]/status")]
    public Task ProxyTaskStatuses() => ProxyRequestAsync($"{apiUrl}/project/status");

    [HttpGet]
    [Route("api/[controller]/{projectId}/task")]
    public Task ProxyTasksByProjectId(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/task");

    [Route("api/[controller]/task/{taskId}/status")]
    public Task ProxyUpdateTaskStatus(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/status");

    [Route("api/[controller]/task/filter/{**rest}")]
    public Task ProxyFilterTasks(string rest) => ProxyRequestAsync($"{apiUrl}/project/task/filter?{rest}");

    [HttpPost]
    [Route("api/[controller]/{projectId}/task")]
    public Task ProxyCreateTask(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/task");
}
