using Cardmngr.Shared.Exceptions;

namespace Cardmngr.Shared.Extensions;

public static class EnumerableExtensions
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

    public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2) where T : IEquatable<T>
    {
        var cnt = new Dictionary<T, int>();
        foreach (T s in list1) 
        {
            if (cnt.TryGetValue(s, out int value)) 
            {
                cnt[s] = ++value;
            } 
            else 
            {
                cnt.Add(s, 1);
            }
        }
        foreach (T s in list2) 
        {
            if (cnt.TryGetValue(s, out int value)) 
            {
                cnt[s] = --value;
            } 
            else 
            {
                return false;
            }
        }
        return cnt.Values.All(c => c == 0);
    }
}
