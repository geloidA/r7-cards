namespace Onlyoffice.Api.Models.Common;

public class FilterBuilder
{
    protected readonly Dictionary<string, string> _filters = [];

    public readonly static FilterBuilder Empty = new();

    public FilterBuilder Count(int count)
    {
        _filters["count"] = count.ToString();
        return this;
    }

    public FilterBuilder StartIndex(int startIndex)
    {
        _filters["startIndex"] = startIndex.ToString();
        return this;
    }

    public FilterBuilder SortBy(string sortBy)
    {
        _filters["sortBy"] = sortBy;
        return this;
    }

    public FilterBuilder SortOrder(FilterSortOrders sortOrder)
    {
        _filters["sortOrder"] = sortOrder.ToFilterSortOrder();
        return this;
    }

    public FilterBuilder FilterBy(string filterBy)
    {
        _filters["filterBy"] = filterBy;
        return this;
    }

    public FilterBuilder FilterOperator(FilterOperators filterOperator)
    {
        _filters["filterOperator"] = filterOperator.ToFilterOperator();
        return this;
    }

    public FilterBuilder Simple()
    {
        _filters["simple"] = "true";
        return this;
    }

    public FilterBuilder FilterValue(string filterValue)
    {
        _filters["filterValue"] = filterValue;
        return this;
    }

    public virtual string Build()
    {
        return $"{string.Join("&", _filters.Select(x => $"{x.Key}={x.Value}"))}";
    }

    public T AsChild<T>() where T : FilterBuilder
    {
        return (T)this;
    }
}
