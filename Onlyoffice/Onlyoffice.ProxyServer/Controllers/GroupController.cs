using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public class GroupController(IConfiguration conf) : ApiController(conf)
{
    [HttpGet("api/[controller]")]
    public Task ProxyGetGroups() => ProxyRequestAsync($"{apiUrl}/group");
}
