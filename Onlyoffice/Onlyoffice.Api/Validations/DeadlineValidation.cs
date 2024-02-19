using System.ComponentModel.DataAnnotations;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Validations.TaskValidation;

public static class DeadlineValidation
{
    public static ValidationResult? CheckDeadline(DateTime? deadline, ValidationContext context)
    {
        var model = (TaskUpdateData)context.ObjectInstance;

        if (deadline < model.StartDate)
        {
            return new ValidationResult("Крайний срок меньше дня начала", [context.MemberName!] );
        }

        return ValidationResult.Success;
    }
}
