using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public abstract class UserTaskFilterBase : IFilter<OnlyofficeTask>
{
    protected UserTaskFilterBase(UserInfo? user)
    {
        User = user;   
    }

    protected UserTaskFilterBase(string userName)
    {
        UserName = userName;
    }

    protected readonly UserInfo? User;
    protected readonly string? UserName;

    public abstract bool Filter(OnlyofficeTask item);
}
