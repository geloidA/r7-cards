using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class CreatedByTaskFilter : UserTaskFilterBase
{
    public CreatedByTaskFilter(UserInfo user) : base(user)
    {
    }

    public CreatedByTaskFilter(string userName) : base(userName)
    {
        
    }

    public override bool Filter(OnlyofficeTask item)
    {
        return User is { }
            ? item.CreatedBy.Id == User.Id
            : item.CreatedBy.DisplayName.Contains(UserName!, StringComparison.CurrentCultureIgnoreCase);
    }

    public override string ToString() => "CreatedByTaskFilter: " + UserName;
}
