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
        var client = httpClientFactory.CreateClient("api");
        
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

    public async Task<AuthenticationResponseType> LoginAsync(LoginModel login)
    {
        var client = httpClientFactory.CreateClient("api");
        string payload = JsonSerializer.Serialize(login);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response;

        try
        {
            response = await client.PostAsync("api/authentication", content);
        }
        catch (HttpRequestException)
        {
            return AuthenticationResponseType.Error;
        }

        
        return response.IsSuccessStatusCode ? AuthenticationResponseType.Success : AuthenticationResponseType.Error;
    }
}
