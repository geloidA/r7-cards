using System.Net.Http.Headers;
using Cardmngr.Exceptions;

namespace Cardmngr.Services;

public class AppInfoService(HttpClient http)
{
    public async Task<string> GetVersionAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/appinfo/version")
        {
            Headers = { CacheControl = new CacheControlHeaderValue { NoCache = true } }
        };

        var response = await http.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new CheckAppVersionException(response.ReasonPhrase!);
    }
}
