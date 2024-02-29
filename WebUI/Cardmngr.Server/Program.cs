using System.Net;
using System.Security.Cryptography.X509Certificates;
using Cardmngr.Server.AppInfoApi.Service;
using Cardmngr.Shared.Extensions;
using Cardmngr.Server.Extensions;
using Cardmngr.Server.FeedbackApi.Service;
using Cardmngr.Server.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(new CompactJsonFormatter(), builder.Configuration["Logging:pathFormat"]!, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddSignalR();

builder.Services.AddControllers();

ConfigureFeedbackDirectory(builder.Configuration);

builder.Services
    .AddSingleton<GroupManager>()
    .AddScoped<IFeedbackService, FeedbackService>()
    .AddScoped<IAppInfoService, AppInfoService>();

builder.WebHost.UseKestrel(opt => 
{
    var config = opt.ApplicationServices.GetRequiredService<IConfiguration>();
    var certificatePath = config.CheckKey("CertificateSettings:CertificatePublic");
    var keyCertificate = config.CheckKey("CertificateSettings:CertificatePrivate");

    var port = int.Parse(config.CheckKey("Port"));
        
    opt.Listen(IPAddress.Parse(config["IPAddress"]!), port, listenOptions =>
    {
        listenOptions.UseHttps(X509Certificate2.CreateFromPemFile(certificatePath, keyCertificate));
    });
});

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
    app.UseResponseCompression();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapRazorPages();

app.MapHubs();

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