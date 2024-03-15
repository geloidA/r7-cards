namespace Cardmngr.Shared.Utils.Filter;

public interface IFilterManager<T> : IFilterManager
{
    new IEnumerable<IFilter<T>> Filters { get; }

    void AddFilter(IFilter<T> filter);
    bool RemoveFilter(IFilter<T> filter);

    void Clear();
}

public interface IFilterManager
{
    IEnumerable<IFilter> Filters { get; }
    event Action? FilterChanged;
}
