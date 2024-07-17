using Cardmngr.Exceptions;

namespace Cardmngr.Services;

public class AppInfoService(HttpClient http)
{
    public Task<string> GetVersionAsync(string current)
    {
        return http.GetStringAsync($"/appinfo/version/{current}")
            ?? throw new CheckAppVersionException("App version not found");
    }
}
