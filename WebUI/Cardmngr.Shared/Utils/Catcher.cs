namespace Cardmngr.Shared.Utils;

public static class Catcher
{
    public static void Catch<TException>(Action action, Action<TException>? handler = null) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException ex)
        {
            handler?.Invoke(ex);
        }
    }

    public static async Task CatchAsync<TException>(Func<Task> func, Action<TException>? handler = null) where TException : Exception
    {
        try
        {
            await func();
        }
        catch (TException ex)
        {
            handler?.Invoke(ex);
        }
    }

    public static TValue Catch<TException, TValue>(Func<TValue> func, TValue defaultValue = default!) where TException : Exception
    {
        try
        {
            return func();
        }
        catch (TException)
        {
            return defaultValue;
        }
    }

    public static async Task<TValue> CatchAsync<TException, TValue>(Func<Task<TValue>> func, TValue defaultValue = default!) where TException : Exception
    {
        try
        {
            return await func();
        }
        catch (TException)
        {
            return defaultValue;
        }
    }

    public static async ValueTask<TValue> CatchAsync<TException, TValue>(Func<ValueTask<TValue>> func, TValue defaultValue = default!) where TException : Exception
    {
        try
        {
            return await func();
        }
        catch (TException)
        {
            return defaultValue;
        }
    }
}
