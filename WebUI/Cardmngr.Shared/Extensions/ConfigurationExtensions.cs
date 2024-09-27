using Cardmngr.Shared.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Cardmngr.Shared.Extensions;

public static class ConfigurationExtensions
{
    
    /// <summary>
    /// Retrieves the value associated with the given key from the configuration.
    /// Throws <see cref="NotConfiguredConfigException"/> if the key is not present.
    /// </summary>
    /// <param name="config">The configuration to retrieve from.</param>
    /// <param name="key">The key to retrieve.</param>
    /// <returns>The value associated with the given key.</returns>
    public static string CheckKey(this IConfiguration config, string key)
    {
        return config[key] ?? throw new NotConfiguredConfigException(key);
    }
}
