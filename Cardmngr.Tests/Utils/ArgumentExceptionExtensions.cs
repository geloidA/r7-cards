namespace Cardmngr.Tests;

public static class ArgumentExceptionWrapper
{
    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (!values.Any())
        {
            throw new ArgumentException("Collection is empty");
        }
    }
}
