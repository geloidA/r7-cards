using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter;

public class ResponsibleTaskFilter(UserInfo user) : UserTaskFilterBase(user)
{
    public override bool Filter(OnlyofficeTask item)
    {
        return item.Responsibles.Any(x => x.Id == User.Id);
    }
}
