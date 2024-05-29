using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Shared;

public abstract class UserTaskFilterBase : IFilter<OnlyofficeTask>
{
    public UserTaskFilterBase(UserInfo? user)
    {
        User = user;   
    }

    public UserTaskFilterBase(string userName)
    {
        UserName = userName;
    }

    public readonly UserInfo? User;
    public readonly string? UserName;

    public abstract bool Filter(OnlyofficeTask item);
}
