using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils.Filter;

namespace Cardmngr.Shared;

public abstract class UserTaskFilterBase(UserInfo user) : IFilter<OnlyofficeTask>
{
    public readonly UserInfo User = user;

    public abstract bool Filter(OnlyofficeTask item);
}
