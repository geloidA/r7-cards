using System.Net;
using System.Security.Cryptography.X509Certificates;
using AspNetCore.Proxy;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json");
        var config = builder.Configuration;

        builder.Services
            .AddProxies()
            .AddHttpClient();

        builder.Services
            .AddHttpClient("NoCookie")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false });

        builder.Services.AddControllers();

        builder.WebHost.UseKestrel(opt => 
        {
            var config = opt.ApplicationServices.GetRequiredService<IConfiguration>();
            var certificatePath = config["CertificateSettings:CertificatePublic"] ?? throw new NullReferenceException();
            var keyCertificate = config["CertificateSettings:CertificatePrivate"];

            var port = int.Parse(config["Port"] ?? throw new Exception("Haven't port in config"));
                
            opt.Listen(IPAddress.Parse(config["IPAddress"]!), port, listenOptions =>
            {                
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2AndHttp3;
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
                .WithOrigins(config["Receiver"] ?? throw new NullReferenceException("Receiver config is null"))
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

        app.MapControllers();

        app.Run();
    }
}