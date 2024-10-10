using System.Net;
using System.Security.Cryptography.X509Certificates;
using Cardmngr.Shared.Extensions;
using Cardmngr.Server.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.AddServiceDefaults();

builder.Services.AddRazorPages();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddSignalR();

builder.Services.AddCardmngrServices();

builder.Services.AddHttpForwarderWithServiceDiscovery();

if (builder.Environment.IsProduction())
{
    builder.WebHost.UseKestrel(ConfigureServer);
    builder.Services.AddWebOptimizer();
}

builder.Services.AddAuthentication();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseResponseCompression();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseWebOptimizer();
}
else
{
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection()
   .UseBlazorFrameworkFiles()
   .UseStaticFiles();

app.MapHubs();

app.MapSelfEndpoints()
   .MapDefaultEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapFallbackToFile("index.html");

if (app.Environment.IsDevelopment())
{
    // перенаправление запросов на соответствующие сервисы созданные aspire-ом.
    app.MapForwarder("/api/feedback/{**catch-all}", "https+http://feedback", "/api/feedback/{**catch-all}");
    app.MapForwarder("/onlyoffice/{**catch-all}", "https+http://onlyoffice", "/{**catch-all}");
}
else
{
    // перенаправление запросов на сервисы в docker-контейнерах. Значения задаются в переменных окружения в файле docker-compose.yml.
    app.MapForwarder("/api/feedback/{**catch-all}", builder.Configuration.CheckKey("FEEDBACK_SERVICE_URL"), "/api/feedback/{**catch-all}");
    app.MapForwarder("/onlyoffice/{**catch-all}", builder.Configuration.CheckKey("PROXY_SERVER_URL"), "/{**catch-all}");
}

app.Run();
return;

// Метод, главная задача которого является установка протокола HTTPS и настройка адресов подключения. Указываются в docker-compose.yml.
// Адреса будут работать только внутри контейнера, поэтому проброс портов также настраивается внутри docker-compose.yml, который генерируется 
// при запуске скрипта release.sh
static void ConfigureServer(KestrelServerOptions opt)
{
    var config = opt.ApplicationServices.GetRequiredService<IConfiguration>();
    
    var certificatePath = config.CheckKey("CertificateSettings:CertificatePublic");
    var keyCertificate = config.CheckKey("CertificateSettings:CertificatePrivate");

    var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';');

    if (urls is null) return;

    foreach (var url in urls)
    {
        var address = Cardmngr.Shared.Utils.BindingAddress.Parse(url);
        var host = IPAddress.Parse(address.Host);
        if (address.Scheme == Uri.UriSchemeHttps)
        {
            opt.Listen(host, address.Port, listenOptions =>
            {
                listenOptions.UseHttps(X509Certificate2.CreateFromPemFile(certificatePath, keyCertificate));
            });
        }
        else if (address.Scheme == Uri.UriSchemeHttp)
        {
            opt.Listen(host, address.Port);
        }
    }
}