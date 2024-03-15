using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Shared;

public class TaskFilterManager : IFilterManager<OnlyofficeTask>
{
    private readonly List<IFilter<OnlyofficeTask>> filters = [];

    public TaskFilterManager()
    {
        
    }

    public TaskFilterManager(IEnumerable<IFilter<OnlyofficeTask>> filters)
    {
        this.filters = filters.ToList();
    }

    public IEnumerable<IFilter<OnlyofficeTask>> Filters
    {
        get
        {
            foreach (var filter in filters)
                yield return filter;
        }
    }

    IEnumerable<IFilter> IFilterManager.Filters => Filters;

    public event Action? FilterChanged;
    private void OnFilterChanged() => FilterChanged?.Invoke();

    public void AddFilter(IFilter<OnlyofficeTask> filter)
    {
        filters.Add(filter);
        OnFilterChanged();
    }

    public bool RemoveFilter(IFilter<OnlyofficeTask> filter)
    {
        if (filters.Remove(filter))
        {
            OnFilterChanged();
            return true;
        }

        return false;
    }

    public void Clear()
    {
        if (filters.Count > 0)
        {
            filters.Clear();
            OnFilterChanged();
        }
    }
}
