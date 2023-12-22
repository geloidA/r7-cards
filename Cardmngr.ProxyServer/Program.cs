using AspNetCore.Proxy;
using Cardmngr.ProxyServer.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddProxies();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseProxies(proxies =>
        {
            // Проксировать запросы авторизации на конкретный сервер
            proxies
                .MapViaHttpCors("api/2.0/authentication", "http://10.5.23.20/api/2.0/authentication") // TODO: Prevent set-cookies
                .MapViaHttpCors("api/2.0/people/@self", "http://10.5.23.20/api/2.0/people/@self")
                .MapViaHttpCors("api/2.0/project", "http://10.5.23.20/api/2.0/project")
                .MapViaHttpCors("api/2.0/project/@self", "http://10.5.23.20/api/2.0/project/@self");
        });

        app.Run();
    }    
}