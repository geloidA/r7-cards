using System.Security.Claims;
using Cardmngr.Network.Models;
using Microsoft.AspNetCore.Components.Authorization;
 

namespace Cardmngr.Network.Providers;

public class CookieStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity());
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public void SetAuthInfo(UserProfile userProfile)
    {
        var identity = new ClaimsIdentity(new[]{
            new Claim(ClaimTypes.Email, Check(userProfile.Email)),
            new Claim(ClaimTypes.Name, $"{userProfile.FirstName} {userProfile.LastName}"),
            new Claim("UserId", Check(userProfile.Id))
        }, "AuthCookie");
    
        claimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void ClearAuthInfo()
    {
        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static string Check(string? val) => val ?? throw new NullReferenceException("One of user profile property is null");
}
