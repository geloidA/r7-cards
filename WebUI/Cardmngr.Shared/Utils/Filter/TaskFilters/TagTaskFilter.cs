using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class TagTaskFilter(string tagName) : IFilter<OnlyofficeTask>
{
    public bool Filter(OnlyofficeTask item)
    {
        return item.Tags.Any(x => x.Name.Contains(tagName, StringComparison.CurrentCultureIgnoreCase));
    }

    public override string ToString() => "TagTaskFilter: " + tagName;
}
