using Cardmngr.Domain.Entities.Base;

namespace Cardmngr.Domain.Entities;

public record UserInfo : EntityBase<string>
{
    public string DisplayName { get; init; }
    public string AvatarSmall { get; init; }
    public string ProfileUrl { get; init; }
}
