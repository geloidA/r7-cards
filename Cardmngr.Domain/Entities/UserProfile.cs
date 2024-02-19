namespace Cardmngr.Domain.Entities;

public record class UserProfile : UserInfo
{
    public string UserName { get; init; }
    public bool IsVisitor { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public int Status { get; init; } // todo: enum
    public int ActivationStatus { get; init; } // todo: enum
    public DateTime? Terminated { get; init; }
    public string Department { get; init; }
    public DateTime WorkFrom { get; init; }
    public string AvatarMedium { get; init; }
    public string Avatar { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsLDAP { get; init; }
    public bool IsOwner { get; init; }
    public bool IsSSO { get; init; }
    public int QuotaLimit { get; init; }
    public int UsedSpace { get; init; }
    public int DocsSpace { get; init; }
    public int MailSpace { get; init; }
    public int TalkSpace { get; init; }
}
