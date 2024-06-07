using System.Collections;

namespace Cardmngr.Shared.Utils.Filter;

public abstract class FilterByMultipleItemBase<TFilter, TItem>(FilterType filterType) : IFilterByMultipleItem<TFilter, TItem>
{
    private readonly FilterType filterType = filterType;
    private readonly List<TFilter> _filterItems = [];

    public int Count => _filterItems.Count;

    public bool IsReadOnly => false;

    public event Action? FilterChanged;
    private void OnFilterChanged() => FilterChanged?.Invoke();

    public FilterByMultipleItemBase(IEnumerable<TFilter> items, FilterType filterType) : this(filterType)
    {
        _filterItems = items.ToList();
    }

    public void Add(TFilter filterItem)
    {
        if (_filterItems.Contains(filterItem))
        {
            throw new InvalidOperationException("Filter item already exists");
        }
        
        _filterItems.Add(filterItem);
        OnFilterChanged();
    }

    public bool Remove(TFilter filterItem)
    {
        if (_filterItems.Remove(filterItem))
        {
            OnFilterChanged();
            return true;
        }

        return false;
    }

    public void Toggle(TFilter filterItem)
    {
        if (!_filterItems.Remove(filterItem))
        {
            _filterItems.Add(filterItem);
        }

        OnFilterChanged();
    }

    public bool Contains(TFilter filterItem) => _filterItems.Contains(filterItem);
    
    public void Clear()
    {
        _filterItems.Clear();
        OnFilterChanged();
    }

    public bool Filter(TItem item)
    {
        if (_filterItems.Count == 0) return true;
        
        return filterType switch
        {
            FilterType.Exist => _filterItems.Any(x => FilterItem(x, item)),
            FilterType.All => _filterItems.All(x => FilterItem(x, item)),
            _ => throw new NotSupportedException(nameof(filterType))
        };
    }
    
    public abstract bool FilterItem(TFilter filterItem, TItem item);

    public IEnumerator<TFilter> GetEnumerator()
    {
        foreach (var filterItem in _filterItems)
            yield return filterItem;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyTo(TFilter[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public void RemoveRange(IEnumerable<TFilter> filters)
    {
        if (_filterItems.RemoveAll(x => filters.Contains(x)) > 0)
        {
            OnFilterChanged();
        }
    }
}