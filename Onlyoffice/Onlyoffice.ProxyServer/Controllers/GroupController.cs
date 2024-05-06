using Microsoft.AspNetCore.Mvc;
using Onlyoffice.ProxyServer.Controllers;

namespace Onlyoffice.ProxyServer;

public class GroupController(IConfiguration conf) : ApiController(conf)
{
    [HttpGet("api/[controller]")]
    public Task ProxyGetGroups() => ProxyRequestAsync($"{apiUrl}/group");
}
