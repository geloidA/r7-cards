using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Domain.Enums;
using Cardmngr.Shared.Feedbacks;

namespace Cardmngr.Application.Clients.FeedbackClient;

/// <summary>
/// Клиент для работы с отзывами.
/// </summary>
public interface IFeedbackClient
{
    /// <summary>
    /// Получает отзывы.
    /// </summary>
    /// <returns>отзывы.</returns>
    Task<FeedbacksVm> GetFeedbacksAsync();

    /// <summary>
    /// Создает отзыв.
    /// </summary>
    /// <param name="requestData">данные для создания отзыва.</param>
    /// <returns>созданный отзыв.</returns>
    Task<Feedback> CreateFeedbackAsync(FeedbackCreateRequestData requestData);

    /// <summary>
    /// Удаляет отзыв.
    /// </summary>
    /// <param name="feedbackId">идентификатор отзыва, который нужно удалить.</param>
    /// <returns>удаленный отзыв.</returns>
    Task<Feedback?> DeleteFeedbackAsync(int feedbackId);

    /// <summary>
    /// Обновляет отзыв.
    /// </summary>
    /// <param name="feedbackId">идентификатор отзыва, который нужно обновить.</param>
    /// <param name="data">данные для обновления отзыва.</param>
    /// <returns>обновленный отзыв.</returns>
    Task<Feedback?> UpdateFeedbackAsync(int feedbackId, FeedbackUpdateData data);

    /// <summary>
    /// Обновляет статус отзыва.
    /// </summary>
    /// <param name="feedbackId">идентификатор отзыва, статус которого нужно обновить.</param>
    /// <param name="status">новый статус отзыва.</param>
    /// <returns>обновленный отзыв.</returns>
    Task<Feedback> UpdateFeedbackStatusAsync(int feedbackId, FeedbackStatus status);

    /// <summary>
    /// Переключает лайк отзыва.
    /// </summary>
    /// <param name="feedbackId">идентификатор отзыва, лайк которого нужно переключить.</param>
    /// <returns>отзыв.</returns>
    Task<Feedback?> ToggleFeedbackLikeAsync(int feedbackId);

    /// <summary>
    /// Переключает дизлайк отзыва.
    /// </summary>
    /// <param name="feedbackId">идентификатор отзыва, дизлайк которого нужно переключить.</param>
    /// <returns>отзыв.</returns>
    Task<Feedback?> ToggleFeedbackDislikeAsync(int feedbackId);
}
