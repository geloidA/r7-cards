using System.Net.Http.Json;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.People;

public class ApiPeopleRepository(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IPeopleRepository
{
    public async IAsyncEnumerable<UserProfileDto> GetAllAsync()
    {
        var response = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<UserProfilesDao>(ApiPaths.People));
        foreach (var user in response?.Response ?? [])
            yield return user;
    }

    public async Task<UserProfileDto?> GetByIdAsync(string userId)
    {
        var response = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<UserProfileDao>($"{ApiPaths.People}/{userId}"));        
        return response?.Response;
    }

    public async IAsyncEnumerable<UserProfileDto> GetFiltredAsync(FilterBuilder builder)
    {
        var response = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<UserProfilesDao>($"{ApiPaths.People}/filter/{builder.Build()}"));
        foreach (var user in response?.Response ?? [])
            yield return user;
    }
}
