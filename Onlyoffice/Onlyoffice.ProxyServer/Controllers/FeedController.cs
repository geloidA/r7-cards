using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public class FeedController(IConfiguration conf) : ApiController(conf)
{
    [Route("api/[controller]/filter/{**rest}")]
    public Task ProxyFilter(string rest) => ProxyRequestAsync($"{ApiUrl}/feed/filter?{rest}");
}