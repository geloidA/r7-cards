using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities.Feedback;

public sealed record class Feedback : ResponseEntityBase<int>
{
    public string Title { get; init; }
    public string Description { get; init; }
    public UserInfo Creator { get; init; }
    public FeedbackStatus Status { get; init; }
    public bool CanChangeStatus { get; init; }
    public DateTime Created { get; init; }
    public DateTime? Finished { get; init; }
    public List<string> LikedUsers { get; init; } = [];
    public List<string> DislikedUsers { get; init; } = [];

    public bool Equals(Feedback other)
    {
        if (other is null || GetType() != other.GetType()) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id;
}