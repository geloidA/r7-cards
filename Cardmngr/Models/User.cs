using Cardmngr.Models;
using Onlyoffice.Api.Models;

namespace Cardmngr;

public class User(IUser user) : ModelBase, IUser
{
    public string Id { get; } = user.Id ?? throw new NullReferenceException("Id is null");
    public string? AvatarSmall { get; set; } = user.AvatarSmall;
    public string? DisplayName { get; set; } = user.DisplayName;
    public string? ProfileUrl { get; set; } = user.ProfileUrl;
}
