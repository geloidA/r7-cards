using Cardmngr.Domain.Entities;

namespace Cardmngr.Server.UserInfoService;

public interface IUserInfoService
{
    Task<UserInfo?> GetUserInfoAsync(string guid);
}
