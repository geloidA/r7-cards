namespace Cardmngr.Application.Extensions;

public static class IAsyncEnumerableExtensions
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
}
