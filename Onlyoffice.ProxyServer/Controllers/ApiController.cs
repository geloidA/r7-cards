using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public abstract class ApiController(IConfiguration conf) : Controller
{
    protected readonly string apiUrl = conf["ApiUrl"] ?? throw new NullReferenceException("ApiUrl config is null");
    
    protected virtual IHttpProxyOptionsBuilder HttpProxyOptionsBuilder { get; } 
        = AspNetCore.Proxy.Options.HttpProxyOptionsBuilder.Instance
            .WithAfterReceive(ChangeCORSHeaders);

    protected Task ProxyRequestAsync(string destination) => this.HttpProxyAsync(destination, HttpProxyOptionsBuilder.Build());

    private static Task ChangeCORSHeaders(HttpContext _, HttpResponseMessage response)
    {
        response.Headers.Remove("Access-Control-Allow-Origin");
        response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5176");
        response.Headers.Add("Access-Control-Allow-Credentials", "true");
        return Task.CompletedTask;
    }
}
