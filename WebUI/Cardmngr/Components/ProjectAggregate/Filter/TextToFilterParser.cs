using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Components.ProjectAggregate.Filter;

public class TextToFilterParser
{
    private readonly Func<string, IFilter> _defaultFilter;
    private readonly Dictionary<string, Func<string, IFilter>> _filtersByPrefix;
    private static readonly char[] Separators = [' '];

    /// <param name="filters">Filters to search</param>
    /// <param name="defaultFilter">Used when no prefix is found</param>
    /// <exception cref="ArgumentException"></exception>
    public TextToFilterParser(IEnumerable<IFilterModel> filters, Func<string, IFilter> defaultFilter)
    {
        var filterModels = filters.ToList();
        if (filters is null || filterModels.Count == 0) 
            throw new ArgumentException("'filters' cannot be null or empty", nameof(filters));

        _filtersByPrefix = filterModels.ToDictionary(x => x.Prefix, x => new Func<string, IFilter>(x.GetFilter));
        _defaultFilter = defaultFilter;
    }

    public IEnumerable<IFilter> Parse(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            yield break;
        }

        foreach (var filterSegment in text.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
        {
            var splitSegment = filterSegment.Split(':', 2);

            if (splitSegment.Length == 1)
            {
                yield return _defaultFilter(splitSegment[0]);
            }
            else if (_filtersByPrefix.TryGetValue(splitSegment[0], out var filterFactory))
            {
                yield return filterFactory(splitSegment[1]);
            }
        }
    }
}
