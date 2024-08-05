namespace Onlyoffice.Api.Models.Common;

public class FilterBuilder
{
    protected readonly Dictionary<string, string> Filters = [];

    public static readonly FilterBuilder Empty = new();

    public FilterBuilder Count(int count)
    {
        Filters["count"] = count.ToString();
        return this;
    }

    public FilterBuilder StartIndex(int startIndex)
    {
        Filters["startIndex"] = startIndex.ToString();
        return this;
    }

    public FilterBuilder SortBy(string sortBy)
    {
        Filters["sortBy"] = sortBy;
        return this;
    }

    public FilterBuilder SortOrder(FilterSortOrders sortOrder)
    {
        Filters["sortOrder"] = sortOrder.ToFilterSortOrder();
        return this;
    }

    public FilterBuilder FilterBy(string filterBy)
    {
        Filters["filterBy"] = filterBy;
        return this;
    }

    public FilterBuilder FilterOperator(FilterOperators filterOperator)
    {
        Filters["filterOperator"] = filterOperator.ToFilterOperator();
        return this;
    }

    public FilterBuilder Simple()
    {
        Filters["simple"] = "true";
        return this;
    }

    public FilterBuilder FilterValue(string filterValue)
    {
        Filters["filterValue"] = filterValue;
        return this;
    }

    public virtual string Build()
    {
        return $"{string.Join("&", Filters.Select(x => $"{x.Key}={x.Value}"))}";
    }

    public T AsChild<T>() where T : FilterBuilder
    {
        return (T)this;
    }
}
