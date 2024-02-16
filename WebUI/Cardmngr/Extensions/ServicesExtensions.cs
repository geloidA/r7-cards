using Blazored.Modal;
using Microsoft.AspNetCore.Components;

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
}
