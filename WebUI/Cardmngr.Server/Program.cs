using System.Net;
using System.Security.Cryptography.X509Certificates;
using Cardmngr.Shared.Extensions;
using Cardmngr.Server.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorPages();

Log.Logger = new LoggerConfiguration().CreateMyLogger(builder.Configuration.CheckKey("Logging:pathFormat"));

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddSignalR();

builder.Services.AddCardmngrServices();

builder.Services.AddHttpForwarderWithServiceDiscovery();

builder.WebHost.UseKestrel(opt => 
{
    var config = opt.ApplicationServices.GetRequiredService<IConfiguration>();
    var certificatePath = config.CheckKey("CertificateSettings:CertificatePublic");
    var keyCertificate = config.CheckKey("CertificateSettings:CertificatePrivate");
    
    opt.Listen(IPAddress.Any, 8080);
    opt.Listen(IPAddress.Any, 8443, listenOptions =>
    {
        listenOptions.UseHttps(X509Certificate2.CreateFromPemFile(certificatePath, keyCertificate));
    });
});

builder.Services.AddAuthentication();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseResponseCompression();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
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

app.MapForwarder("/api/feedback/{**catch-all}", "https+http://feedback", "/api/feedback/{**catch-all}");
app.MapForwarder("/onlyoffice/{**catch-all}", "https+http://onlyoffice", "/{**catch-all}");

app.Run();