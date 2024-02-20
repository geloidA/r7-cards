using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Logics.People;
using Onlyoffice.Api.Models;

namespace Cardmngr.Server.UserInfoService;

public class UserInfoService(IPeopleApi peopleApi) : IUserInfoService
{
    private readonly IPeopleApi peopleApi = peopleApi;

    public async Task<UserInfo?> GetUserInfoAsync(string guid)
    {
        var userProfile = await peopleApi.GetProfileByIdAsync(guid);

        return userProfile != null
            ? new UserInfo
            {
                Id = userProfile.Id,
                AvatarSmall = userProfile.AvatarSmall,
                ProfileUrl = userProfile.ProfileUrl,
                DisplayName = userProfile.DisplayName
            }
            : null;
    }
}
