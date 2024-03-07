using FluentValidation;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Validations;

public class TaskUpdateDataValidator : AbstractValidator<TaskUpdateData>
{
    public TaskUpdateDataValidator()
    {
        RuleFor(x => x.Deadline)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("Крайний срок меньше дня начала");
            
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Название не может быть пустым");
    }
}
