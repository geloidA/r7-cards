using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Modals;

public abstract class CreationModalBase : ComponentBase
{
    [CascadingParameter] protected BlazoredModalInstance BlazoredModal { get; set; } = default!;
    
    protected virtual async Task SubmitCreate() => await BlazoredModal.CloseAsync(ModalResult.Ok());
    protected virtual async Task Cancel() => await BlazoredModal.CancelAsync();
}
