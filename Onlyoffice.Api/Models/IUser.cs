namespace Onlyoffice.Api.Models;

public interface IUser
{
    public string? Id { get; set; }
    public string? AvatarSmall { get; set; }
    public string? DisplayName { get; set; }
}
