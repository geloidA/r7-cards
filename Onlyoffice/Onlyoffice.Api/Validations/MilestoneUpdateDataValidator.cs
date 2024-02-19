using FluentValidation;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Validations;

public class MilestoneUpdateDataValidator : AbstractValidator<MilestoneUpdateData>
{
    public MilestoneUpdateDataValidator()
    {
        RuleFor(x => x.Deadline).NotNull().WithMessage("Конечный срок не может быть пустым");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Название не может быть пустым");
        RuleFor(x => x.Responsible).NotEmpty().WithMessage("Ответственное лицо не может быть пустым");
    }
}
