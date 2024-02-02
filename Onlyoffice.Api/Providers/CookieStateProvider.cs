using System.Security.Claims;
using Onlyoffice.Api.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Task = System.Threading.Tasks.Task;
using System.Text.Json;

namespace Onlyoffice.Api.Providers;

public class CookieStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity());
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public void SetAuthInfo(UserProfile userProfile)
    {
        var identity = new ClaimsIdentity([
            new Claim(ClaimTypes.Email, Check(userProfile.Email)),
            new Claim(ClaimTypes.Name, $"{userProfile.FirstName} {userProfile.LastName}"),
            new Claim("UserId", Check(userProfile.Id)),
            new Claim("IsAdmin", userProfile.IsAdmin.ToString()),
            new Claim("Data", JsonSerializer.Serialize(userProfile))
        ], "AuthCookie");
    
        claimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public string this[string claim]
    {
        get => claimsPrincipal.FindFirst(claim)?.Value ?? throw new NullReferenceException("One of user profile property is null");
    }

    public void ClearAuthInfo()
    {
        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static string Check(string? val) => val ?? throw new NullReferenceException("One of user profile property is null");
}
