using AspNetCore.Proxy.Builders;

namespace Onlyoffice.ProxyServer.Extensions;

public static class ProxiesBuilderExtensions
{
    public static IProxiesBuilder MapViaHttpCors(this IProxiesBuilder builder, string route, string endpoint)
    {
        builder.Map(route, proxy => proxy.UseHttp(endpoint, 
                opt => opt.WithAfterReceive(ChangeCORSHeaders)));
        return builder;
    }

    private static Task ChangeCORSHeaders(HttpContext _, HttpResponseMessage response)
    {
        response.Headers.Remove("Access-Control-Allow-Origin");
        response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5176");
        response.Headers.Add("Access-Control-Allow-Credentials", "true");
        return Task.CompletedTask;
    }
}
