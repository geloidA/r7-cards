using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Authentication;

namespace Onlyoffice.Api.Logics;

public interface IAuthApiLogic
{
    /// <summary>
    /// Try login with username and password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>Token that expires in 5 hours</returns>
    Task<HttpResponseMessage> LoginAsync(LoginModel login);

    /// <summary>
    /// Get self profile info
    /// </summary>
    /// <returns></returns>
    Task<UserProfileDao?> GetSelfProfileAsync();
}
