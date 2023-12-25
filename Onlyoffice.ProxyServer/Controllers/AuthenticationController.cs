using AspNetCore.Proxy;
using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public class AuthenticationController(IConfiguration conf) : ApiController(conf)
{
    [Route("api/[controller]")]
    public Task ProxyLogin()
    {
        return this.HttpProxyAsync($"{apiUrl}/authentication", HttpProxyOptionsBuilder.Build());
    }
}
