using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Cardmngr.Components.Modals.DetailModals;

public abstract class ValidationComponent : ComponentBase
{
    [Inject] public ToastService ToastService { get; set; } = null!;

    public void NotifyInvalidInput(EditContext context) => ToastService.Notify(new(ToastType.Danger, "Ошибка ввода", GetMessage(context)));

    private static string GetMessage(EditContext context)
    {
        return context
            .GetValidationMessages()
            .Aggregate((x, y) => $"{x}. {y}");
    }
}
