using FluentValidation;

namespace Cardmngr.Shared.Feedbacks;

public class FeedbackUpdateDataValidator : AbstractValidator<FeedbackUpdateData>
{
    public FeedbackUpdateDataValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Название не может быть пустым");
    }
}
