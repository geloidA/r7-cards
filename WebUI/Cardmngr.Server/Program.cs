using System.Net;
using System.Security.Cryptography.X509Certificates;
using Cardmngr.Shared.Extensions;
using Cardmngr.Server.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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

// builder.WebHost.UseKestrel(opt => 
// {
//     var config = opt.ApplicationServices.GetRequiredService<IConfiguration>();
//     var certificatePath = config.CheckKey("CertificateSettings:CertificatePublic");
//     var keyCertificate = config.CheckKey("CertificateSettings:CertificatePrivate");

//     var port = int.Parse(config.CheckKey("Port"));
        
//     opt.Listen(IPAddress.Parse(config["IPAddress"]!), port);
//     opt.Listen(IPAddress.Parse(config["IPAddress"]!), port + 1, listenOptions =>
//     {
//         listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
//         listenOptions.UseHttps(X509Certificate2.CreateFromPemFile(certificatePath, keyCertificate));
//     });
// });

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