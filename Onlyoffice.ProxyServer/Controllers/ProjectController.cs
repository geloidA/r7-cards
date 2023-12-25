using AspNetCore.Proxy;
using Microsoft.AspNetCore.Mvc;
namespace Onlyoffice.ProxyServer.Controllers;

public class ProjectController(IConfiguration conf) : ApiController(conf)
{
    [Route("api/[controller]")]
    public Task ProxyProject()
    {
        return this.HttpProxyAsync($"{apiUrl}/project", HttpProxyOptionsBuilder.Build());
    }

    [Route("api/[controller]/status")]
    public Task ProxyTaskStatuses()
    {
        return this.HttpProxyAsync($"{apiUrl}/project/status", HttpProxyOptionsBuilder.Build());
    }

    [Route("api/[controller]/{projectId}/task")]
    public Task ProxyTasksByProjectId(int projectId)
    {
        return this.HttpProxyAsync($"{apiUrl}/project/{projectId}/task", HttpProxyOptionsBuilder.Build());
    }
}
