using Cardmngr.Domain.Feedback;

namespace Cardmngr.Server.Exceptions;

public class FeedbackNotFoundException(Feedback feedback) : Exception($"Can't find feedback by id - {feedback.Id}")
{

}
