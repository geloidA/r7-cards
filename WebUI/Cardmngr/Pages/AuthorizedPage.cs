using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Cardmngr.Application.Extensions;

namespace Cardmngr;

[Authorize]
public abstract class AuthorizedPage : ComponentBase
{
    protected bool IsAuthenticated { get; private set; }
    protected string? IdOnInitialization { get; private set; }
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;
    [CascadingParameter] protected Task<AuthenticationState>? AuthenticationState { get; set; }
    
    protected async override Task OnInitializedAsync()
    {
        if (AuthenticationState is { })
        {
            var user = await AuthenticationState;
            IsAuthenticated = user.User.Identity!.IsAuthenticated;
            if (!user.User.Identity!.IsAuthenticated)
            {
                NavigationManager.NavigateTo("login");
                return;
            }
            IdOnInitialization = user.User.GetNameIdentifier();
        }
    }
}
