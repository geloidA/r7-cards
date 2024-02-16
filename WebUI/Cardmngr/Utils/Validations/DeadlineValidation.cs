using System.ComponentModel.DataAnnotations;

namespace Cardmngr;

public static class DeadlineValidation
{
    public static ValidationResult? CheckDeadline(DateTime? deadline, ValidationContext context)
    {
        // var model = (IWork)context.ObjectInstance;

        // if (deadline < model.StartDate)
        // {
        //     return new ValidationResult("Крайний срок меньше дня начала", [context.MemberName!] );
        // }

        return ValidationResult.Success;
    }
}
