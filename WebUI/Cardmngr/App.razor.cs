using System.Security.Claims;
using Blazored.LocalStorage;
using Cardmngr.Domain.Entities;
using Cardmngr.Notification;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Onlyoffice.Api.Providers;
using Onlyoffice.Api.Logics;
using Cardmngr.Application.Clients;

namespace Cardmngr;

public partial class App : ComponentBase
{
    [Inject] ILocalStorageService LocalStorage { get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationProvider { get; set; } = null!;
    [Inject] IAuthApiLogic AuthApiLogic { get; set; } = null!;
    [Inject] AppInfoService AppInfoService { get; set; } = null!;
    [Inject] NotificationService NotificationService { get; set; } = null!;
    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] ProjectSummaryService ProjectSummaryService { get; set; } = null!;
    [Inject] NotificationHubConnection NotificationHubConnection { get; set; } = null!;
    [Inject] IJSRuntime JS { get; set; } = null!;

    private async Task OnNavigateAsync(NavigationContext args)
    {
        var auth =  await LocalStorage.GetItemAsync<string>("isauthenticated");
        var user = await AuthenticationProvider.GetAuthenticationStateAsync();

        if (!string.IsNullOrEmpty(auth) && !user.User.Identity!.IsAuthenticated)
        {
            var userDao = await AuthApiLogic.GetSelfProfileAsync();
            if (userDao is { })
            {
                AuthenticationProvider
                    .ToCookieProvider()
                    .SetAuthInfo(userDao.Response!);
            }
            else 
            {
                await LocalStorage.RemoveItemAsync("isauthenticated");
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {        
        AuthenticationProvider.AuthenticationStateChanged += RequestPermission;
        NotificationHubConnection.TaskReceived += ReceiveTask;
        var appVersion = await AppInfoService.GetVersionAsync();

        ProjectSummaryService.FollowedProjectIds = new HashSet<int>(await ProjectClient.GetFollowedProjectsAsync()
                                                                                       .Select(x => x.Id).ToListAsync());
        
        await JS.InvokeVoidAsync("updateVersion", appVersion);

        await NotificationService.RequestPermissionAsync();
    }

    private async void RequestPermission(Task<AuthenticationState> authState)
    {
        var state = await authState;

        if (state?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value is { } userId)
        {
            if (NotificationHubConnection.Connected)
            {
                await NotificationHubConnection.DisposeAsync();
            }

            await NotificationHubConnection.StartAsync(userId);
        }
    }

    private async void ReceiveTask(OnlyofficeTask task)
    {
        var options = new NotificationOptions
        {
            Body = $"От: {task.CreatedBy.DisplayName}\nПоручена задача - {task.Title}\nВ проекте: {task.ProjectOwner.Title}",
            Href = $"/project/{task.ProjectOwner.Id}",
            Icon = "/favicon.png",
            RequireInteraction = true
        };

        await NotificationService.CreateAsync("Появилась новая задача!", options);
    }
}