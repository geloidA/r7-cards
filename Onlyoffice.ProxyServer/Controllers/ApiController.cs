using AspNetCore.Proxy;
using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public abstract class ApiController(IConfiguration conf) : Controller
{
    protected readonly string receiver = conf["Receiver"] ?? throw new NullReferenceException("Reciver config is null");
    protected readonly string apiUrl = conf["ApiUrl"] ?? throw new NullReferenceException("ApiUrl config is null");

    protected Task ProxyRequestAsync(string destination) => this.HttpProxyAsync(destination);
}
