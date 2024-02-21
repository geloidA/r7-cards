using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Server.UserInfoService;

public class UserInfoService(IHttpClientFactory httpClientFactory) : IUserInfoService
{
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

    public async Task<UserInfo?> GetUserInfoAsync(string guid)
    {
        using var client = httpClientFactory.CreateClient("onlyoffice");
        var userProfile = await client.GetFromJsonAsync<UserProfileDao>($"people/{guid}");

        return userProfile?.Response != null
            ? new UserInfo
            {
                Id = userProfile.Response.Id,
                AvatarSmall = userProfile.Response.AvatarSmall,
                ProfileUrl = userProfile.Response.ProfileUrl,
                DisplayName = userProfile.Response.DisplayName
            }
            : null;
    }
}
