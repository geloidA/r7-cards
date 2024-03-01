using Serilog;
using Serilog.Formatting.Compact;

namespace Cardmngr.Shared.Extensions;

public static class LoggerConfigurationExtensions
{
    public static ILogger CreateMyLogger(this LoggerConfiguration loggerConfiguration, string pathFormat)
    {
        return loggerConfiguration
            .WriteTo.File(new CompactJsonFormatter(), pathFormat, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
