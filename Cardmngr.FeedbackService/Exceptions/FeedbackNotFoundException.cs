
using Cardmngr.Domain.Entities.Feedback;

namespace Cardmngr.FeedbackService.Exceptions;

public class FeedbackNotFoundException(Feedback feedback) : Exception($"Can't find feedback by id - {feedback.Id}")
{

}
