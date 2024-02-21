using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Feedback;

public record class Feedback : ResponseEntityBase<int> 
{
    public string Title { get; init; }
    public string Description { get; init; }
    public UserInfo Creator { get; init; }
    public FeedbackStatus Status { get; init; }
    public bool CanChangeStatus { get; init; }
}