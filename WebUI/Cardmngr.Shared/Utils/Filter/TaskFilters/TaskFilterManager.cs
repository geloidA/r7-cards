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
        _filters = [.. filters];
    }

    public ICollection<IFilter<OnlyofficeTask>> Filters => _filters;

    ICollection<IFilter> IFilterManager.Filters => [.. Filters.Cast<IFilter>()];

    public event Action? FilterChanged;
    private void OnFilterChanged() => FilterChanged?.Invoke();

    public void AddFilter(IFilter<OnlyofficeTask> filter) => AddFilters([filter]);

    public void Reset(IEnumerable<IFilter<OnlyofficeTask>> filters)
    {
        Clear(notify: false);
        AddFilters(filters);
    }

    public bool RemoveFilter(IFilter<OnlyofficeTask> filter)
    {
        if (_filters.Remove(filter))
        {
            if (filter is IChangeableFilter changeableFilter)
            {
                changeableFilter.FilterChanged -= OnFilterChanged;
            }

            OnFilterChanged();
            return true;
        }

        return false;
    }

    public void Clear() => Clear(true);

    private void Clear(bool notify = false)
    {
        if (_filters.Count > 0)
        {
            foreach (var filter in _filters)
            {
                if (filter is IChangeableFilter changeableFilter)
                {
                    changeableFilter.FilterChanged -= OnFilterChanged;
                }
            }

            _filters.Clear();

            if (notify) OnFilterChanged();
        }
    }

    public int RemoveFilters(IEnumerable<IFilter<OnlyofficeTask>> filters, IEqualityComparer<IFilter>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(nameof(filters));

        comparer ??= new FilterTypeEqualityComparer();

        ICollection<IFilter<OnlyofficeTask>> containedFilters = [.. _filters.Where(x => filters.Contains(x, comparer))];

        foreach (var filter in containedFilters)
        {
            if (filter is IChangeableFilter changeableFilter)
            {
                changeableFilter.FilterChanged -= OnFilterChanged;
            }

            _filters.Remove(filter);
        }

        if (containedFilters.Count > 0)
            OnFilterChanged();
        
        return containedFilters.Count;
    }

    public void AddFilters(IEnumerable<IFilter<OnlyofficeTask>> filters)
    {
        if (!filters.Any()) return;

        _filters.AddRange(filters);

        foreach (var filter in filters)
        {
            if (filter is IChangeableFilter changeableFilter)
            {
                changeableFilter.FilterChanged += OnFilterChanged;
            }
        }

        OnFilterChanged();    
    }
}
