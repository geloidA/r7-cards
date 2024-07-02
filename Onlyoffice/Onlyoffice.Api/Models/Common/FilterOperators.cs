namespace Onlyoffice.Api.Models.Common;

public enum FilterOperators
{
    Contains,
    Equals,
    StartWith,
    Present
}

public static class FilterOperatorsExtensions
{
    public static string ToFilterOperator(this FilterOperators filterOperator)
    {
        return filterOperator switch
        {
            FilterOperators.Contains => "contains",
            FilterOperators.Equals => "equals",
            FilterOperators.StartWith => "startWith",
            FilterOperators.Present => "present",
            _ => throw new ArgumentOutOfRangeException(nameof(filterOperator), filterOperator, null)
        };
    }
}
