namespace Cardmngr.Application.Extensions;

public static class EnumerableExtensions
{
    public static ValueTask<List<TResult>> ToListAsync<TSource, TResult>(
        this IAsyncEnumerable<TSource> source, 
        Func<TSource, TResult> selector,
        CancellationToken cancellationToken = default)
    {
        return source
            .Select(selector)
            .ToListAsync(cancellationToken);
    }

    public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        return source
            .Select(selector)
            .ToList();
    }
}
