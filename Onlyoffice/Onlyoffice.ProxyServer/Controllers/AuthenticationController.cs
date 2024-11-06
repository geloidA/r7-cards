using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public class AuthenticationController(IConfiguration conf) : ApiController(conf)
{
    [Route("api/[controller]")]
    public Task ProxyLogin()
    {
        return ProxyRequestAsync($"{ApiUrl}/authentication", HttpProxyOptionsBuilder.Instance.WithHttpClientName("NoCookie"));
    }

    [HttpPost("api/[controller]/logout")]
    public Task Logout()
    {
        HttpContext.Response.Cookies.Append("asc_auth_key", "", new CookieOptions 
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(-1)
        });

        return Task.CompletedTask;
    }
}
