using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Feedback;
using Cardmngr.Shared.Feedbacks;

namespace Cardmngr.Server.FeedbackApi.Service;

public interface IFeedbackService
{
    IAsyncEnumerable<Feedback> GetFeedbacks(string requestGuid);
    Task<Feedback> CreateFeedbackAsync(FeedbackUpdateData data, UserInfo user);
    Task<Feedback?> ToggleFeedbackLikeAsync(Feedback feedback, string requestGuid);
    Task<Feedback?> ToggleFeedbackDislikeAsync(Feedback feedback, string requestGuid);
    Task<Feedback?> FindFeedbackAsync(int id);
    Task<Feedback> UpdateFeedbackAsync(Feedback feedback, FeedbackUpdateData data, string requestGuid);
    Task<Feedback> DeleteFeedbackAsync(Feedback feedback);
    bool CanManipulate(string guid, Feedback feedback);
    Task<Feedback?> UpdateFeedbackStatusAsync(Feedback feedback, FeedbackStatus status, string requestGuid);
}
