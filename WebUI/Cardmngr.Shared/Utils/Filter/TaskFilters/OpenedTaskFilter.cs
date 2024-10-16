using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class OpenedTaskFilter : IFilter<OnlyofficeTask>
{
    public bool Filter(OnlyofficeTask item)
    {
        return item.Status != Domain.Enums.Status.Closed;
    }
}
