using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class TagTaskFilter(string tagsName) : IFilter<OnlyofficeTask>
{
    public bool Filter(OnlyofficeTask item)
    {
        return tagsName
            .Split(',')
            .Any(tag => item.Tags.Any(x => x.Name.Contains(tag, StringComparison.CurrentCultureIgnoreCase)));
    }

    public override string ToString() => "TagTaskFilter: " + tagsName;
}
