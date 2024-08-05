using System.Net;

namespace Onlyoffice.ProxyServer.Extensions;

public static class CookiesExtensions
{
    // Returns an HttpClient with cookies added based on the provided IRequestCookieCollection, apiUrl, and allowedKeys.
    // If allowedKeys is empty, all cookies will be added.
    public static HttpClient GetClientFor(this IRequestCookieCollection cookies, string apiUrl, params string[] allowedKeys)
    {
        var handler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer()
        };

        foreach (var cookie in cookies.Where(x => allowedKeys.Length == 0 || allowedKeys.Contains(x.Key)))
        {
            handler.CookieContainer.Add(new Uri(apiUrl), new Cookie(cookie.Key, cookie.Value));
        }
        
        return new HttpClient(handler);
    }
}
