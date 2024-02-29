using Cardmngr.Shared.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Cardmngr.Shared.Extensions;

public static class ConfigurationExtensions
{
    public static string CheckKey(this IConfiguration config, string key)
    {
        return config[key] ?? throw new NotConfiguredConfigException(key);
    }
}
