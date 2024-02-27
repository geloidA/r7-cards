using System.Net.Http.Headers;
using Cardmngr.Exceptions;

namespace Cardmngr.Services;

public class AppInfoService(IHttpClientFactory httpClientFactory)
{
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

    public async Task<string> GetVersionAsync()
    {
        using var client = httpClientFactory.CreateClient("self-api");

        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{client.BaseAddress}/appinfo/version")
        };

        message.Headers.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true
        };

        var response = await client.SendAsync(message);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new CheckAppVersionException(response.ReasonPhrase!);
    }
}
