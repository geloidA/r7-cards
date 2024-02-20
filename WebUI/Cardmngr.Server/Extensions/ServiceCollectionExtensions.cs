using Cardmngr.Server.Exceptions;
using Onlyoffice.Api.Handlers;

namespace Cardmngr.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder ConfigureOnlyofficeClient(this IServiceCollection services, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var directoryPath = Path.GetFullPath(config["FeedbackConfig:directory"] 
            ?? throw new NotConfiguredConfigException("FeedbackConfig:directory"));
        
        DirectoryWrapper.CreateIfDoesntExists(directoryPath);

        if (!File.Exists($"{directoryPath}/feedbacks.json"))
        {
            File.WriteAllText($"{directoryPath}/feedbacks.json", "[]");
        }

        var onlyofficeApi = config["OnlyofficeConfig:apiUrl"] ?? throw new NotConfiguredConfigException("OnlyofficeConfig:apiUrl");

        return services
            .AddHttpClient("onlyoffice", opt => opt.BaseAddress = new Uri(onlyofficeApi))
            .AddHttpMessageHandler<CookieHandler>();
    }
}
