using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class CreatedByTaskFilter(UserInfo user) : UserTaskFilterBase(user)
{
    public override bool Filter(OnlyofficeTask item)
    {
        return item.CreatedBy.Id == User.Id;
    }
}
