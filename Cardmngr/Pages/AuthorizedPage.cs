using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cardmngr;

[Authorize]
public abstract class AuthorizedPage : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    
    protected async override Task OnInitializedAsync()
    {
        if (AuthenticationState is { })
        {
            var user = await AuthenticationState;
            if (!user.User.Identity!.IsAuthenticated)
            {
                NavigationManager.NavigateTo("login");
                return; // TODO: Show error
            }
        }
    }
}
