using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Domain.Enums;
using Cardmngr.Shared.Feedbacks;

namespace Cardmngr.FeedbackService.Services;

/// <summary>
/// Интерфейс для работы с отзывами.
/// </summary>
public interface IFeedbackService
{
    /// <summary>
    /// Получает список отзывов.
    /// </summary>
    /// <param name="requestGuid">GUID авторизованного пользователя.</param>
    /// <returns>Список отзывов.</returns>
    IAsyncEnumerable<Feedback> GetFeedbacks(string requestGuid);

    /// <summary>
    /// Создает новый отзыв.
    /// </summary>
    /// <param name="data">Данные о создаваемом отзыве.</param>
    /// <param name="user">Информация о пользователе, который создает отзыв.</param>
    /// <returns>Созданный отзыв.</returns>
    Task<Feedback> CreateFeedbackAsync(FeedbackUpdateData data, UserInfo user);

    /// <summary>
    /// Переключает лайк отзыва.
    /// </summary>
    /// <param name="feedback">Отзыв, который будет изменен.</param>
    /// <param name="requestGuid">GUID авторизованного пользователя.</param>
    /// <returns>Измененный отзыв.</returns>
    Task<Feedback?> ToggleFeedbackLikeAsync(Feedback feedback, string requestGuid);

    /// <summary>
    /// Переключает дизлайк отзыва.
    /// </summary>
    /// <param name="feedback">Отзыв, который будет изменен.</param>
    /// <param name="requestGuid">GUID авторизованного пользователя.</param>
    /// <returns>Измененный отзыв.</returns>
    Task<Feedback?> ToggleFeedbackDislikeAsync(Feedback feedback, string requestGuid);

    /// <summary>
    /// Ищет отзыв по ID.
    /// </summary>
    /// <param name="id">ID отзыва, который будет найден.</param>
    /// <returns>Отзыв, если он был найден, null - если не найден.</returns>
    Task<Feedback?> FindFeedbackAsync(int id);

    /// <summary>
    /// Обновляет отзыв.
    /// </summary>
    /// <param name="feedback">Отзыв, который будет изменен.</param>
    /// <param name="data">Данные, которые будут использованы для обновления.</param>
    /// <param name="requestGuid">GUID авторизованного пользователя.</param>
    /// <returns>Обновленный отзыв.</returns>
    Task<Feedback> UpdateFeedbackAsync(Feedback feedback, FeedbackUpdateData data, string requestGuid);

    /// <summary>
    /// Удаляет отзыв.
    /// </summary>
    /// <param name="feedback">Отзыв, который будет удален.</param>
    /// <returns>Удаленный отзыв.</returns>
    Task<Feedback> DeleteFeedbackAsync(Feedback feedback);

    /// <summary>
    /// Проверяет может ли пользователь изменять отзыв.
    /// </summary>
    /// <param name="guid">GUID авторизованного пользователя.</param>
    /// <param name="feedback">Отзыв, которые будет изменен.</param>
    /// <returns>True - может, false - не может.</returns>
    bool CanManipulate(string guid, Feedback feedback);

    /// <summary>
    /// Обновляет статус отзыва.
    /// </summary>
    /// <param name="feedback">Отзыв, который будет изменен.</param>
    /// <param name="status">Статус, на который будет изменен отзыв.</param>
    /// <param name="requestGuid">GUID авторизованного пользователя.</param>
    /// <returns>Обновленный отзыв.</returns>
    Task<Feedback?> UpdateFeedbackStatusAsync(Feedback feedback, FeedbackStatus status, string requestGuid);
}
