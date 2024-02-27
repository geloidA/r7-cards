using Cardmngr.Server.AppInfoApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace Cardmngr.Server.AppInfoApi;

[ApiController]
[Route("api/[controller]")]
public class AppInfoController(IAppInfoService appInfoService) : Controller
{
    private readonly IAppInfoService appInfoService = appInfoService;

    [HttpGet("version")]
    public async Task<string> GetVersion() 
    {
        return await appInfoService.GetVersionAsync();
    }
}
