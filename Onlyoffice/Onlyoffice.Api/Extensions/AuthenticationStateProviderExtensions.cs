using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Providers;

namespace Onlyoffice.Api.Extensions;

public static class AuthenticationStateProviderExtensions
{
    public static CookieStateProvider ToCookieProvider(this AuthenticationStateProvider provider) => (CookieStateProvider)provider;
}
