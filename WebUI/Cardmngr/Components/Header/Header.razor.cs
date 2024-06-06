using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Domain.Entities;
using KolBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using Onlyoffice.Api.Extensions;
using Cardmngr.Components.Header.Modals;
using Cardmngr.Shared.Extensions;
using Cardmngr.Utils;

namespace Cardmngr.Components.Header;

public partial class Header : KolComponentBase
{
    [Inject] public IConfiguration Config { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ILocalStorageService LocalStorage { get; set; } = null!;

    [CascadingParameter] HeaderInteractionService InteractionService { get; set; } = null!;

    string proxyUrl = null!;
    string onlyofficeUrl = null!;

    private UserProfile? currentUser;

    [CascadingParameter(Name = "DetailsModal")] ModalOptions Options { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (AuthenticationState is { })
        {
            proxyUrl = Config.CheckKey("proxy-url");
            onlyofficeUrl = Config.CheckKey("onlyoffice-url");
            var provider = AuthenticationState.ToCookieProvider();
            currentUser = JsonSerializer.Deserialize<UserProfile>(provider["Data"]);
            InteractionService.HeaderCollapsedChanged += StateHasChanged;
        }
    }

    private async Task OpenSidebar()
    {
        await Modal.Show<HeaderMenuModal>("", new ModalParameters { { "Nothing", new object() } }, Options).Result;
            // TODO: ??? close animation only with parameter ???
    }

    private async Task OpenSettings()
    {
        await Modal.Show<SettingsModal>("", new ModalParameters { { "Nothing", new object() } }, Options).Result;
    }

    async Task LogoutAsync()
    {
        if (AuthenticationState is { })
        {            
            AuthenticationState.ToCookieProvider().ClearAuthInfo();
            await LocalStorage.RemoveItemAsync("isauthenticated");
            NavigationManager.NavigateTo("login");
        }
    }
}
