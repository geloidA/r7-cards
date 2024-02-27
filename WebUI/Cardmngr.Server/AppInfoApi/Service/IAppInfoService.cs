namespace Cardmngr.Server.AppInfoApi.Service;

public interface IAppInfoService
{
    Task<string> GetVersionAsync();
}
