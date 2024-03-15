namespace Cardmngr.Shared.Utils.Filter;

public interface IFilterByMultipleItem<TFilter, T> : IFilter<T>, IEnumerable<TFilter>
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
