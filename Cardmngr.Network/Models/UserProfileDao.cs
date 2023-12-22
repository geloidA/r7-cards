namespace Cardmngr.Network.Models;

public class UserProfileDao : HttpResponseDaoBase
{
    public UserProfile? Response { get; set; }
}

public class UserProfile
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public bool IsVisitor { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int Status { get; set; }
    public int ActivationStatus { get; set; }
    public DateTime? Terminated { get; set; }
    public string? Department { get; set; }
    public DateTime WorkFrom { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarMedium { get; set; }
    public string? Avatar { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsLDAP { get; set; }
    public bool IsOwner { get; set; }
    public bool IsSSO { get; set; }
    public string? AvatarSmall { get; set; }
    public int QuotaLimit { get; set; }
    public int UsedSpace { get; set; }
    public int DocsSpace { get; set; }
    public int MailSpace { get; set; }
    public int TalkSpace { get; set; }
    public string? ProfileUrl { get; set; }
}