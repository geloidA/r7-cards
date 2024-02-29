using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public class AuthenticationController(IConfiguration conf) : ApiController(conf)
{
    [Route("api/[controller]")]
    public Task ProxyLogin()
    {
        return ProxyRequestAsync($"{apiUrl}/authentication", HttpProxyOptionsBuilder.Instance.WithHttpClientName("NoCookie"));
    }
}
