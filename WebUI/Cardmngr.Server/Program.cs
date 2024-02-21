using System.Net;
using System.Security.Cryptography.X509Certificates;
using Cardmngr.Server.Exceptions;
using Cardmngr.Server.Extensions;
using Cardmngr.Server.FeedbackApi.Service;
using Cardmngr.Server.UserInfoService;
using Microsoft.AspNetCore.ResponseCompression;
using Onlyoffice.Api.Handlers;
using Onlyoffice.Api.Logics.People;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddControllers();

ConfigureFeedbackDirectory(builder.Configuration);

builder.Services.AddScoped<IFeedbackService, FeedbackService>();

builder.WebHost.UseKestrel(opt => 
{
    var config = opt.ApplicationServices.GetRequiredService<IConfiguration>();
    var certificatePath = config["CertificateSettings:CertificatePublic"] ?? throw new NullReferenceException();
    var keyCertificate = config["CertificateSettings:CertificatePrivate"];

    var port = int.Parse(config["Port"] ?? throw new Exception("Haven't port in config"));
        
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

app.MapFallbackToFile("index.html");

app.Run();

static void ConfigureFeedbackDirectory(IConfiguration config)
{
    ArgumentNullException.ThrowIfNull(config);

    var directoryPath = Path.GetFullPath(config["FeedbackConfig:directory"] 
        ?? throw new NotConfiguredConfigException("FeedbackConfig:directory"));
    
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