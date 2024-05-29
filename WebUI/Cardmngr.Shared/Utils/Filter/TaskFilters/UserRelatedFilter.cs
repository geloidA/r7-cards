using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class UserRelatedFilter(string userId) : IFilter<OnlyofficeTask>
{
    public bool Filter(OnlyofficeTask item)
    {
        return item.CreatedBy.Id == userId ||
               item.Responsibles.Any(x => x.Id == userId) ||
               item.Subtasks.Any(x => x.CreatedBy.Id == userId);
    }
}
