using Cardmngr.Domain.Feedback;
using Cardmngr.Shared.Feedbacks;

namespace Cardmngr.Application.Clients.FeedbackClient;

public interface IFeedbackClient
{
    Task<FeedbacksVm> GetFeedbacksAsync();
    Task<Feedback> CreateFeedbackAsync(FeedbackUpdateData feedback);
}
