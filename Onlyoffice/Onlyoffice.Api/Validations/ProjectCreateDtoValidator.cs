using FluentValidation;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Validations;

public class ProjectCreateDtoValidator : AbstractValidator<ProjectCreateDto>
{
    public ProjectCreateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Название не может быть пустым");
        RuleFor(x => x.ResponsibleId).NotNull().WithMessage("Выберите менеджера проекта");
    }
}