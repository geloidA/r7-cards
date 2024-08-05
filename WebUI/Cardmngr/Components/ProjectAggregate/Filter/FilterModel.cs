using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Components.ProjectAggregate.Filter;

public class FilterModel(string prefix, Func<string, IFilter> getFilter, string? example = null) : IFilterModel
{
    public string Prefix { get; set; } = prefix;
    public string? Example { get; set; } = example;
    public IFilter GetFilter(string text) => getFilter(text);
}

public interface IFilterModel
{
    string Prefix { get; set; }
    string? Example { get; set; }
    IFilter GetFilter(string text);
}
