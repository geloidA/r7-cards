using Cardmngr.Network.Models;
using Cardmngr.Network.Models.Authentication;

namespace Cardmngr.Network.Logics;

public interface IAuthApiLogic
{
    /// <summary>
    /// Try login with username and password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>Token that expires in 5 hours</returns>
    Task<AuthenticationResponseType> LoginAsync(LoginModel login);

    /// <summary>
    /// Get self profile info
    /// </summary>
    /// <returns></returns>
    Task<UserProfileDao?> GetSelfProfileAsync();
}
