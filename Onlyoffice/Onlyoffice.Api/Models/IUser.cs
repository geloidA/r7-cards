namespace Onlyoffice.Api.Models;

public interface IUser
{
    public string? Id { get; }
    public string? AvatarSmall { get; set; }
    public string? DisplayName { get; set; }
    public string? ProfileUrl { get; set; }
}
