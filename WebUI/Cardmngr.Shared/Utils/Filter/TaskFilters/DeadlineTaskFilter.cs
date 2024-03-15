using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Extensions;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class DeadlineTaskFilter : IFilter<OnlyofficeTask>
{
    public bool Filter(OnlyofficeTask item)
    {
        return item.IsDeadlineOut();
    }
}
