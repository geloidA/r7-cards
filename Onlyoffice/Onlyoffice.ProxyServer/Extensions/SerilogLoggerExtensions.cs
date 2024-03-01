namespace Onlyoffice.ProxyServer.Extensions;

public static class SerilogLoggerExtensions
{
    public static void LogInfoWithConnection(this Serilog.ILogger logger, string method, ConnectionInfo connection)
    {
        logger.Information(method + ". Connection: {connection}", connection.RemoteIpAddress);
    }
}
