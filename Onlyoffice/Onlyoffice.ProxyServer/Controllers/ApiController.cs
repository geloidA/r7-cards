using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Onlyoffice.ProxyServer.Controllers;

public abstract class ApiController : Controller
{
    private readonly Serilog.ILogger logger;
    protected readonly string receiver;
    protected readonly string serverUrl;
    protected readonly string apiUrl;

    public ApiController(IConfiguration conf)
    {
        receiver = conf.CheckKey("Receiver");
        serverUrl = conf.CheckKey("ServerUrl");
        apiUrl = conf.CheckKey("ApiUrl");
        logger = Log.Logger.ForContext(GetType());
    }

    protected Task ProxyRequestAsync(string destination, IHttpProxyOptionsBuilder? builder = null)
    {
        LogInfo(destination);
        return this.HttpProxyAsync(destination, builder?.Build());
    }

    private void LogInfo(string destination)
    {
        logger.Information("Method: {method}. Request: {destination}.",
            HttpContext.Request.Method, 
            destination);
    }
}
