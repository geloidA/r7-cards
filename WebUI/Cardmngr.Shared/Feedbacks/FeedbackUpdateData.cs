using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Feedbacks;

public record class FeedbackCreateRequestData(UserInfo User, FeedbackUpdateData Data);

public class FeedbackUpdateData
{
    public string? Title { get; set; }
    public string? Description { get; set; }    
}
