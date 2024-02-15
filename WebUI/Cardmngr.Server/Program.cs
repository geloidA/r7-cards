using System.Net;
using System.Security.Cryptography.X509Certificates;
using Cardmngr.Server.Extensions;
using Cardmngr.Server.Services;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

DirectoryWrapper.CreateIfDoesntExists(Path.GetFullPath(config["FeedbackDirectory"] ?? throw new Exception("FeedbackDirectory config is null")));

builder.Services
    .AddScoped<IProjectFileService, ProjectFileService>()
    .AddRazorPages();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddControllers();

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