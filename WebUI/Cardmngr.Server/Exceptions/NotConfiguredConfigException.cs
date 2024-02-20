namespace Cardmngr.Server.Exceptions;

public class NotConfiguredConfigException(string fieldName) : Exception($"{fieldName} is not configured")
{
}
