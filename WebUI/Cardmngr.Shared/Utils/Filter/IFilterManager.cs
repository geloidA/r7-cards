namespace Cardmngr.Shared.Utils.Filter;

public interface IFilterManager<T> : IFilterManager
{
    new ICollection<IFilter<T>> Filters { get; }

    void AddFilter(IFilter<T> filter);
    bool RemoveFilter(IFilter<T> filter);

    void Reset(IEnumerable<IFilter<T>> filters);

    int RemoveFilters(IEnumerable<IFilter<T>> filters, IEqualityComparer<IFilter>? comparer = null);

    void AddFilters(IEnumerable<IFilter<T>> filters);

    void Clear();
}

public interface IFilterManager
{
    ICollection<IFilter> Filters { get; }
    event Action? FilterChanged;
}
