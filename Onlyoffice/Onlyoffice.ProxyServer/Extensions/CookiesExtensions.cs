using System.Net;

namespace Onlyoffice.ProxyServer.Extensions;

public static class CookiesExtensions
{
    public static HttpClient GetClientFor(this IRequestCookieCollection cookies, string apiUrl)
    {
        var handler = new HttpClientHandler
        {
            CookieContainer = new()
        };

        foreach (var cookie in cookies)
        {
            handler.CookieContainer.Add(new Uri(apiUrl), new Cookie(cookie.Key, cookie.Value));
        }
        
        return new HttpClient(handler);
    }
}
