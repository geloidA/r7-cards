using System.Security.Claims;
using Onlyoffice.Api.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using Cardmngr.Domain.Entities;
using AutoMapper;

namespace Onlyoffice.Api.Providers;

public class CookieStateProvider(IMapper mapper) : AuthenticationStateProvider
{
    private ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public void SetAuthInfo(UserProfileDto userProfile)
    {
        var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Email, Check(userProfile.Email)),
                new Claim(ClaimTypes.Name, Check(userProfile.DisplayName)),
                new Claim(ClaimTypes.NameIdentifier, Check(userProfile.Id)),
                new Claim("IsAdmin", userProfile.IsAdmin.ToString()),
                new Claim("Data", JsonSerializer.Serialize(userProfile))
            ], "ApplicationCookie"
        );
    
        claimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public string this[string claim] => claimsPrincipal.FindFirst(claim)?.Value ?? throw new NullReferenceException($"{claim} property is null");

    public string UserId => this[ClaimTypes.NameIdentifier];

    public UserProfile? UserInfo
    {
        get
        {
            var userProfile = JsonSerializer.Deserialize<UserProfileDto>(this["Data"]);
            
            return userProfile != null ? mapper.Map<UserProfile>(userProfile) : null;
        }
    }

    private static string Check(string? val) => val ?? throw new NullReferenceException("One of user profile property is null");
}
