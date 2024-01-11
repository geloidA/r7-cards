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

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
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

        app.Run($"http://{config["Host"] ?? throw new NullReferenceException("Host config is null")}");
    }    
}