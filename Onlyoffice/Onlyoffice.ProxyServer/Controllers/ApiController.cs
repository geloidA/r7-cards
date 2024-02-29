using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public abstract class ApiController(IConfiguration conf) : Controller
{
    protected readonly string receiver = conf.CheckKey("Receiver");
    protected readonly string serverUrl = conf.CheckKey("ServerUrl"); 
    protected readonly string apiUrl = conf.CheckKey("ApiUrl");

    protected Task ProxyRequestAsync(string destination) => this.HttpProxyAsync(destination);

    protected Task ProxyRequestAsync(string destination, IHttpProxyOptionsBuilder builder) => this.HttpProxyAsync(destination, builder.Build());
}
