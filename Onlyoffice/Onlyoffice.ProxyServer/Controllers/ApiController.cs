using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Onlyoffice.ProxyServer.Controllers;

public abstract class ApiController : Controller
{
    private readonly Serilog.ILogger logger;
    protected readonly string ServerUrl;
    protected readonly string ApiUrl;

    protected ApiController(IConfiguration conf)
    {
        ServerUrl = conf.CheckKey("ServerUrl");
        ApiUrl = conf.CheckKey("ApiUrl");
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
