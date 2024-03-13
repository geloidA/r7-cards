using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Feedback;
using Cardmngr.Shared.Feedbacks;

namespace Cardmngr.Application.Clients.FeedbackClient;

public interface IFeedbackClient
{
    Task<FeedbacksVm> GetFeedbacksAsync();
    Task<Feedback> CreateFeedbackAsync(FeedbackCreateRequestData requestData);
    Task<Feedback?> DeleteFeedbackAsync(int feedbackId);
    Task<Feedback?> UpdateFeedbackAsync(int feedbackId, FeedbackUpdateData data);
    Task<Feedback> UpdateFeedbackStatusAsync(int feedbackId, FeedbackStatus status);
    Task<Feedback?> ToggleFeedbackLikeAsync(int feedbackId, bool add);
}
