using Microsoft.AspNetCore.Components.Authorization;

namespace Onlyoffice.Api.Providers;

public static class AuthenticationStateProviderExtensions
{
    public static async Task<string?> GetNameAsync(this AuthenticationStateProvider provider)
    {
        var state = await provider.GetAuthenticationStateAsync();

        if (state.User.Identity?.IsAuthenticated ?? false)
        {
            return state.User.Identity.Name;
        }

        return null;
    }

    public static CookieStateProvider ToCookieProvider(this AuthenticationStateProvider provider) => (CookieStateProvider)provider;
}
