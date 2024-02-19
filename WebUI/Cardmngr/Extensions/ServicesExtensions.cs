using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Validations;

namespace Cardmngr;

public static class ServicesExtensions
{
    public static IServiceCollection AddMyCascadingValues(this IServiceCollection services)
    {
        return services
            .AddCascadingValue(sp => new CascadingValueSource<ModalOptions>(
                    "MiddleModal", 
                    new ModalOptions { Position = ModalPosition.Middle }, 
                    true))
            .AddCascadingValue(sp => new CascadingValueSource<ModalOptions>(
                    "DetailsModal", 
                    new ModalOptions 
                    { 
                        Position = ModalPosition.Middle,
                        Size = ModalSize.ExtraLarge,
                        DisableBackgroundCancel = true,
                        UseCustomLayout = true
                    }, 
                    true));
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<TaskUpdateDataValidator>()
            .AddScoped<MilestoneUpdateDataValidator>();
    }
}
