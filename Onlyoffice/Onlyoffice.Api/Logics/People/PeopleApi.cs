using System.Net.Http.Json;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.People;

public class PeopleApi(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IPeopleApi
{
    public async Task<UserProfileDto?> GetProfileByIdAsync(string userId)
    {
        var response = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<UserProfileDao>($"api/people/{userId}"));
        
        return response?.Response;
    }
}
