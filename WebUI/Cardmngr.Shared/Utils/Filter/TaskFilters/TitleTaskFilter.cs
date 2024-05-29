using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class TitleTaskFilter(string titleText) : IFilter<OnlyofficeTask>
{
    public bool Filter(OnlyofficeTask item)
    {
        return item.Title.Contains(titleText, StringComparison.CurrentCultureIgnoreCase);
    }
}
