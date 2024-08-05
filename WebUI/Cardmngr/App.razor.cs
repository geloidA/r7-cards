using System.Security.Claims;
using Blazored.LocalStorage;
using Cardmngr.Application.Clients;
using Cardmngr.Domain.Entities;
using Cardmngr.Notification;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Onlyoffice.Api.Extensions;
using Onlyoffice.Api.Logics;

namespace Cardmngr;

public partial class App : ComponentBase
{
    public const string MessagesNotificationCenter = "messages-notification-center";

    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationProvider { get; set; } = null!;
    [Inject] private IAuthApiLogic AuthApiLogic { get; set; } = null!;
    [Inject] private AppInfoService AppInfoService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private NotificationHubConnection NotificationHubConnection { get; set; } = null!;
    [Inject] private IJSRuntime Js { get; set; } = null!;
    [Inject] private IProjectClient ProjectClient { get; set; } = null!;    
    [Inject] private IFollowedProjectManager FollowedProjectManager { get; set; } = null!;
    [Inject] private ITaskNotificationManager TaskNotificationManager { get; set; } = null!;

    private async Task OnNavigateAsync(NavigationContext args)
    {
        var auth =  await LocalStorage.GetItemAsync<string>("isauthenticated").ConfigureAwait(false);
        var user = await AuthenticationProvider.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (!string.IsNullOrEmpty(auth) && !user.User.Identity!.IsAuthenticated)
        {
            var userDao = await AuthApiLogic.GetSelfProfileAsync().ConfigureAwait(false);
            if (userDao is not null)
            {
                AuthenticationProvider
                    .ToCookieProvider()
                    .SetAuthInfo(userDao.Response!);
            }
            else 
            {
                await LocalStorage.RemoveItemAsync("isauthenticated").ConfigureAwait(false);
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        AuthenticationProvider.AuthenticationStateChanged += RequestPermission;
        NotificationHubConnection.TaskReceived += ReceiveTask;

        var current = (await LocalStorage.GetItemAsync<string>("r7cards-version").ConfigureAwait(false)) ?? "unknown";
        var server = await AppInfoService.GetVersionAsync(current).ConfigureAwait(false);

        await Js.InvokeVoidAsync("updateVersion", current, server).ConfigureAwait(false);

        await NotificationService.RequestPermissionAsync().ConfigureAwait(false);
    }

    private void RequestPermission(Task<AuthenticationState> authState)
    {
        _ = InvokeAsync(async () =>
        {
            var state = await authState.ConfigureAwait(false);

            if (state.User.FindFirst(ClaimTypes.NameIdentifier)?.Value is { } userId)
            {
                if (NotificationHubConnection.Connected)
                {
                    await NotificationHubConnection.DisposeAsync().ConfigureAwait(false);
                }
                
                FollowedProjectManager.Refresh(await ProjectClient
                    .GetFollowedProjectsAsync()
                    .Select(x => x.Id).ToListAsync().ConfigureAwait(false));
                await NotificationHubConnection.StartAsync(userId).ConfigureAwait(false);
                await TaskNotificationManager.NotifyDeadlinesAsync().ConfigureAwait(false);
            }
        });
    }

    private void ReceiveTask(OnlyofficeTask task)
    {
        var options = new NotificationOptions
        {
            Body = $"От: {task.CreatedBy.DisplayName}\nПоручена задача - {task.Title}\nВ проекте: {task.ProjectOwner.Title}",
            Href = $"/project/board?ProjectId={task.ProjectOwner.Id}",
            Icon = "/favicon.png",
            RequireInteraction = true
        };

        TaskNotificationManager.NotifyNew(task);

        _ = InvokeAsync(async () => await NotificationService.CreateAsync("Появилась новая задача!", options).ConfigureAwait(false));
    }
}