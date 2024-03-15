namespace Cardmngr.Shared.Utils.Filter;

public static class FilterExtensions
{
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, IFilter<T> filter)
    {
        return source.Where(filter.Filter);
    }

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, IFilterManager<T> manager)
    {
        return source.Where(x => manager.Filters.All(f => f.Filter(x)));
    }

    public static TFilter OfType<TFilter>(this IFilterManager manager)
        where TFilter : IFilter
    {
        return manager.Filters
            .OfType<TFilter>()
            .Single();
    }
}
