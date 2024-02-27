namespace Cardmngr.Exceptions;

public static class ListExtensions
{
    /// <exception cref="RemoveSingleException"></exception>
    public static void RemoveSingle<T>(this List<T> source, Predicate<T> predicate)
    {
        var count = source.RemoveAll(predicate);

        if (count != 1)
        {
            throw new RemoveSingleException(count);
        }
    }
}
