using Cardmngr.FeedbackService.Extensions;
using Cardmngr.FeedbackService.Services;
using Cardmngr.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile($"appsettings{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

ConfigureFeedbackDirectory(builder.Configuration);

builder.Services.AddScoped<IFeedbackService, FeedbackService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapFeedbackApi();

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
