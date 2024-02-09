using Onlyoffice.Api.Models;

namespace Cardmngr;

public class User : IUser
{
    public string? Id { get; }
    public string? AvatarSmall { get; set; }
    public string? DisplayName { get; set; }
    public string? ProfileUrl { get; set; }

    public User() { }

    public User(IUser user)
    {
        Id = user.Id;
        AvatarSmall = user.AvatarSmall;
        DisplayName = user.DisplayName;
        ProfileUrl = user.ProfileUrl;
    }

    public User(string id)
    {
        Id = id;
    }
}
