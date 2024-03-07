using System.Net;
using System.Security.Cryptography.X509Certificates;
using Cardmngr.Shared.Extensions;
using Cardmngr.Server.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

Log.Logger = new LoggerConfiguration().CreateMyLogger(builder.Configuration.CheckKey("Logging:pathFormat"));

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddSignalR();

builder.Services.AddControllers();

ConfigureFeedbackDirectory(builder.Configuration);

builder.Services.AddCardmngrServices();

builder.WebHost.UseKestrel(opt => 
{
    var config = opt.ApplicationServices.GetRequiredService<IConfiguration>();
    var certificatePath = config.CheckKey("CertificateSettings:CertificatePublic");
    var keyCertificate = config.CheckKey("CertificateSettings:CertificatePrivate");

    var port = int.Parse(config.CheckKey("Port"));
        
    opt.Listen(IPAddress.Parse(config["IPAddress"]!), port);
    opt.Listen(IPAddress.Parse(config["IPAddress"]!), port + 1, listenOptions =>
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

app.UseHttpsRedirection()
   .UseBlazorFrameworkFiles()
   .UseStaticFiles();

app.MapHubs();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

static void ConfigureFeedbackDirectory(IConfiguration config)
{
    ArgumentNullException.ThrowIfNull(config);

    var directoryPath = Path.GetFullPath(config.CheckKey("FeedbackConfig:directory"));
    
    DirectoryWrapper.CreateIfDoesntExists(directoryPath);

    if (!File.Exists($"{directoryPath}/feedbacks.json"))
    {
        File.WriteAllText($"{directoryPath}/feedbacks.json", "[]");
    }

    if (!File.Exists($"{directoryPath}/counter"))
    {
        File.WriteAllText($"{directoryPath}/counter", "0");
    }
}