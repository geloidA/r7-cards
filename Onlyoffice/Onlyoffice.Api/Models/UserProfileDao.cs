namespace Onlyoffice.Api.Models;

public class UserProfilesDao : MultiResponseDao<UserProfileDto>;

public class UserProfileDao : SingleResponseDao<UserProfileDto>;

public class UserProfileDto : IUser, IEntityDto<string?>
{
    public string? Id { get; set; }
    public string? UserName { get; init; }
    public bool IsVisitor { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public int Status { get; init; }
    public int ActivationStatus { get; init; }
    public DateTime? Terminated { get; init; }
    public string? Department { get; init; }
    public DateTime WorkFrom { get; init; }
    public string? DisplayName { get; set; }
    public string? AvatarMedium { get; init; }
    public string? Avatar { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsLdap { get; init; }
    public bool IsOwner { get; init; }
    public bool IsSso { get; init; }
    public string? AvatarSmall { get; set; }
    public long QuotaLimit { get; init; }
    public long UsedSpace { get; init; }
    public long DocsSpace { get; init; }
    public long MailSpace { get; init; }
    public long TalkSpace { get; init; }
    public string? ProfileUrl { get; set; }
}