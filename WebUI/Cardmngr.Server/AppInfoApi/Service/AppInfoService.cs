namespace Cardmngr.Server.AppInfoApi.Service;

public class AppInfoService : IAppInfoService
{
    public async Task<string> GetVersionAsync()
    {
        return await File.ReadAllTextAsync("version.txt");
    }
}
