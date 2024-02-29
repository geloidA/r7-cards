using Cardmngr.Server.AppInfoApi.Service;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Cardmngr.Server.AppInfoApi;

[ApiController]
[Route("api/[controller]")]
public class AppInfoController(IAppInfoService appInfoService) : Controller
{
    private readonly IAppInfoService appInfoService = appInfoService;
    private readonly Serilog.ILogger logger = Log.ForContext<AppInfoController>();

    [HttpGet("version")]
    public async Task<string> GetVersion() 
    {
        logger.Information("GetVersion. Connection: {connection}", HttpContext.Connection.RemoteIpAddress);
        return await appInfoService.GetVersionAsync();
    }
}
