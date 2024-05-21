using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Extensions;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class MutableProjectState : ProjectStateBase, IRefresheableProjectState, IAsyncDisposable
{
    private int previousId = -1;
    private ProjectHubClient hubClient = null!;

    [Parameter] public int Id { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public RefreshService RefreshService { get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private readonly TaskFilterManager taskFilterManager = new();
    public override IFilterManager<OnlyofficeTask> TaskFilter => taskFilterManager;

    protected override void OnInitialized()
    {
        RefreshService.Refreshed += OnRefreshModelAsync;
        RefreshService.Start(TimeSpan.FromSeconds(7));
    }

    public async Task ToggleFollowAsync()
    {
        await ProjectClient.FollowProjectAsync(Project.Id);
        Project.IsFollow = !Project.IsFollow;
    }

    protected override async Task OnParametersSetAsync()
    {
        Initialized = false;

        if (previousId != Id)
        {
            await SetModelAsync(await ProjectClient.GetProjectAsync(Id), true);
            TaskFilter.Clear();
            previousId = Id;

            hubClient = await GetNewHubClientAsync();
            await hubClient.StartAsync();
        }

        Initialized = true;
    }

    private async void OnRefreshModelAsync()
    {
        await SetModelAsync(await ProjectClient.GetProjectAsync(Id), false, false);
    }

    private async Task<ProjectHubClient> GetNewHubClientAsync()
    {
        if (hubClient is { })
        {
            await hubClient.DisposeAsync();
        }

        var client = new ProjectHubClient(NavigationManager, TaskClient, Id, AuthenticationStateProvider.ToCookieProvider());

        client.OnUpdatedTask += UpdateTask;
        client.OnDeletedTask += RemoveTask;
        client.OnCreatedTask += AddTask;

        return client;
    }

    public async ValueTask DisposeAsync()
    {
        if (hubClient is { })
        {
            await hubClient.DisposeAsync();
        }

        RefreshService.Dispose();
    }
}
