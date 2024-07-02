namespace Onlyoffice.Api.Models.Common;

public enum FilterSortOrders
{
    Asc,
    Desc
}

public static class FilterSortOrdersExtensions
{
    public static string ToFilterSortOrder(this FilterSortOrders filterSortOrder)
    {
        return filterSortOrder switch
        {
            FilterSortOrders.Asc => "ascending",
            FilterSortOrders.Desc => "descending",
            _ => throw new ArgumentOutOfRangeException(nameof(filterSortOrder), filterSortOrder, null)
        };
    }
}
