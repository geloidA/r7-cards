using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseResponseCompression();
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapRazorPages();

app.MapFallbackToFile("index.html");


app.Run($"http://0.0.0.0:{config["Port"] ?? throw new NullReferenceException("Port config is null")}");
