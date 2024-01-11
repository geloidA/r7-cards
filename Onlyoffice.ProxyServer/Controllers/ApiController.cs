using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public abstract class ApiController(IConfiguration conf) : Controller
{
    protected readonly string receiver = conf["Receiver"] ?? throw new NullReferenceException("Reciver config is null");
    protected readonly string serverUrl = conf["ServerUrl"] ?? throw new NullReferenceException("ServerUrl config is null"); 
    protected readonly string apiUrl = conf["ApiUrl"] ?? throw new NullReferenceException("ApiUrl config is null");

    protected Task ProxyRequestAsync(string destination) => this.HttpProxyAsync(destination);

    protected Task ProxyRequestAsync(string destination, IHttpProxyOptionsBuilder builder) => this.HttpProxyAsync(destination, builder.Build());
}
