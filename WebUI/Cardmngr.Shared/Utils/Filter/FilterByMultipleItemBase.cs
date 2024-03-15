using System.Collections;

namespace Cardmngr.Shared.Utils.Filter;

public abstract class FilterByMultipleItemBase<TFilter, TItem>(FilterType filterType) : IFilterByMultipleItem<TFilter, TItem>
{
    private readonly FilterType filterType = filterType;
    private readonly List<TFilter> filterItems = [];

    public event Action? FilterChanged;
    private void OnFilterChanged() => FilterChanged?.Invoke();

    public FilterByMultipleItemBase(IEnumerable<TFilter> items, FilterType filterType) : this(filterType)
    {
        filterItems = items.ToList();
    }

    public void Add(TFilter filterItem)
    {
        if (filterItems.Contains(filterItem))
        {
            throw new InvalidOperationException("Filter item already exists");
        }
        
        filterItems.Add(filterItem);
        OnFilterChanged();
    }

    public bool Remove(TFilter filterItem)
    {
        if (filterItems.Remove(filterItem))
        {
            OnFilterChanged();
            return true;
        }

        return false;
    }

    public void Toggle(TFilter filterItem)
    {
        if (!filterItems.Remove(filterItem))
        {
            filterItems.Add(filterItem);
        }

        OnFilterChanged();
    }

    public bool Contains(TFilter filterItem) => filterItems.Contains(filterItem);
    
    public void Clear()
    {
        filterItems.Clear();
        OnFilterChanged();
    }

    public bool Filter(TItem item)
    {
        return filterType switch
        {
            FilterType.Exist => filterItems.Any(x => FilterItem(x, item)),
            FilterType.All => filterItems.All(x => FilterItem(x, item)),
            _ => throw new NotSupportedException(nameof(filterType))
        };
    }
    
    public abstract bool FilterItem(TFilter filterItem, TItem item);

    public IEnumerator<TFilter> GetEnumerator()
    {
        foreach (var filterItem in filterItems)
            yield return filterItem;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}