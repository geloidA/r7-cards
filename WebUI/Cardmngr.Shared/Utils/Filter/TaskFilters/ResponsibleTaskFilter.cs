using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter;

public class ResponsibleTaskFilter : UserTaskFilterBase
{
    public ResponsibleTaskFilter(UserInfo user) : base(user)
    {
        
    }

    public ResponsibleTaskFilter(string userName) : base(userName)
    {
        
    }

    public override bool Filter(OnlyofficeTask item)
    {
        return UserName != null
            ? item.Responsibles.Any(x => x.DisplayName.Contains(UserName, StringComparison.CurrentCultureIgnoreCase))
            : item.Responsibles.Any(x => x.Id == User!.Id);
    }

    public override string ToString() => "ResponsibleTaskFilter: " + UserName;
}
