using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public class PeopleController(IConfiguration conf) : ApiController(conf)
{
    [Route("api/[controller]/@self")]
    public Task ProxySelf() => ProxyRequestAsync($"{apiUrl}/people/@self");
}
