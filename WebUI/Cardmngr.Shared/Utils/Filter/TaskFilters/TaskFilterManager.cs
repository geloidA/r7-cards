using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Shared;

public class TaskFilterManager : IFilterManager<OnlyofficeTask>
{
    private readonly List<IFilter<OnlyofficeTask>> _filters = [];

    public TaskFilterManager()
    {
        
    }

    public TaskFilterManager(IEnumerable<IFilter<OnlyofficeTask>> filters)
    {
        _filters = filters.ToList();
    }

    public ICollection<IFilter<OnlyofficeTask>> Filters => _filters;

    ICollection<IFilter> IFilterManager.Filters => [.. Filters.Cast<IFilter>()];

    public event Action? FilterChanged;
    private void OnFilterChanged() => FilterChanged?.Invoke();

    public void AddFilter(IFilter<OnlyofficeTask> filter)
    {
        _filters.Add(filter);
        OnFilterChanged();
    }

    public void Reset(IEnumerable<IFilter<OnlyofficeTask>> filters)
    {
        _filters.Clear();

        if (filters.Any())
        {
            _filters.AddRange(filters);
        }
        
        OnFilterChanged();
    }

    

    public bool RemoveFilter(IFilter<OnlyofficeTask> filter)
    {
        if (_filters.Remove(filter))
        {
            OnFilterChanged();
            return true;
        }

        return false;
    }

    public void Clear()
    {
        if (_filters.Count > 0)
        {
            _filters.Clear();
            OnFilterChanged();
        }
    }

    public int RemoveFilters(IEnumerable<IFilter<OnlyofficeTask>> filters, IEqualityComparer<IFilter>? comparer = null)
    {
        comparer ??= new FilterTypeEqualityComparer();

        var removed = _filters.RemoveAll(x => filters.Contains(x, comparer));

        if (removed > 0)
            OnFilterChanged();
        
        return removed;
    }

    public void AddFilters(IEnumerable<IFilter<OnlyofficeTask>> filters)
    {
        _filters.AddRange(filters);
        OnFilterChanged();
    }
}
