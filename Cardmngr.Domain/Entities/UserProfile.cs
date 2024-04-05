namespace Cardmngr.Domain.Entities;

public record UserProfile(
    string UserName,
    bool IsVisitor,
    string Email,
    int Status,
    int ActivationStatus,
    DateTime? Terminated,
    string Department,
    DateTime WorkFrom,
    string AvatarMedium,
    string Avatar,
    bool IsAdmin,
    bool IsLDAP,
    bool IsOwner,
    bool IsSSO,
    int QuotaLimit,
    int UsedSpace,
    int DocsSpace,
    int MailSpace,
    int TalkSpace,
    string FirstName = "Unknown",
    string LastName = "Unknown"
) : UserInfo;
