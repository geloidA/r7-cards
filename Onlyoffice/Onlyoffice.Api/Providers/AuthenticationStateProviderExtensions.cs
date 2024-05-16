using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Providers;

namespace Onlyoffice.Api.Extensions;

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
