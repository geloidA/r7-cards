using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.ResponseCompression;
using Cardmngr.Shared.Extensions;
using Serilog;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().CreateMyLogger(builder.Configuration.CheckKey("Logging:pathFormat"));

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddResponseCompression(opts =>
{
    opts.EnableForHttps = true;
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services
    .AddHttpClient("NoCookie")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false });

builder.Services.AddControllers();

var config = builder.Configuration;

builder.WebHost.UseKestrel(opt =>
{
    var config = opt.ApplicationServices.GetRequiredService<IConfiguration>();
    var certificatePath = config.CheckKey("CertificateSettings:CertificatePublic");
    var keyCertificate = config.CheckKey("CertificateSettings:CertificatePrivate");

    var port = int.Parse(config.CheckKey("Port"));

    opt.Listen(IPAddress.Parse(config["IPAddress"]!), port, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps(X509Certificate2.CreateFromPemFile(certificatePath, keyCertificate));
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseResponseCompression();
    app.UseHsts();
}

app.UseCors(builder =>
{
    builder
        .WithOrigins(config.CheckKey("Receiver"))
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.MapControllers();

app.Run();
