using FluentValidation;

namespace Cardmngr.Validators;

public class LoginDataValidator : AbstractValidator<LoginData>
{
    public LoginDataValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Поле не может быть пустым");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Поле не может быть пустым");
    }
}

public class LoginData
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}
