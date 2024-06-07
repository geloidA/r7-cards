namespace Cardmngr.Shared.Utils.Filter;

public interface IFilterByMultipleItem<TFilter, T> : IFilter<T>, ICollection<TFilter>, IChangeableFilter
{
    void RemoveRange(IEnumerable<TFilter> filters);
}

public interface IChangeableFilter : IFilter
{
    event Action? FilterChanged;
}

public interface IFilter<in T> : IFilter
{
    bool Filter(T item);
}

public interface IFilter
{
    
}
