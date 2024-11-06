using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Authentication;

namespace Onlyoffice.Api.Logics;

public class AuthApiLogic(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IAuthApiLogic
{
    public async Task<UserProfileDao?> GetSelfProfileAsync()
    {
        using var client = HttpClientFactory.CreateClient("onlyoffice");
        
        HttpResponseMessage response;

        try
        {
            response = await client.GetAsync("api/people/@self");
        }
        catch (HttpRequestException)
        {
            return null;
        }
        
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<UserProfileDao>() : null;
    }

    public async Task<HttpResponseMessage> LoginAsync(LoginModel login)
    {
        using var client = HttpClientFactory.CreateClient("onlyoffice");
        string payload = JsonSerializer.Serialize(login);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        return await client.PostAsync("api/authentication", content);
    }

    public async Task LogoutAsync()
    {
        using var client = HttpClientFactory.CreateClient("onlyoffice");
        await client.PostAsync("api/authentication/logout", null);
    }
}
