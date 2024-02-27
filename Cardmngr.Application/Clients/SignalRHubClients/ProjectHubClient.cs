using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Onlyoffice.Api.Providers;

namespace Cardmngr.Application.Clients.SignalRHubClients;

public class ProjectHubClient : IAsyncDisposable
{
    private readonly string userIdentity;
    private readonly ITaskClient taskClient;
    private readonly int projectId;
    private readonly HubConnection connection;

    public ProjectHubClient(NavigationManager navigationManager, 
        ITaskClient taskClient,
        int projectId,
        CookieStateProvider authStateProvider)
    {
        connection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/hubs/projectboard"))
            .Build();

        connection.On<int>(nameof(ReceiveCreatedTaskAsync), ReceiveCreatedTaskAsync);
        connection.On<int>(nameof(ReceiveUpdatedTaskAsync), ReceiveUpdatedTaskAsync);
        connection.On<int>(nameof(ReceiveDeletedTaskAsync), ReceiveDeletedTaskAsync);

        connection.On<string>(nameof(JoinGroupMemberAsync), JoinGroupMemberAsync);
        connection.On<string>(nameof(LeaveGroupMemberAsync), LeaveGroupMemberAsync);

        userIdentity = authStateProvider.UserInfo?.Id 
            ?? throw new UnauthorizedAccessException("User is not logged in");

        this.projectId = projectId;
        this.taskClient = taskClient;
    }

    public event Action? ConnectedMembersChanged;

    private void LeaveGroupMemberAsync(string userId)
    {
        if (connectedMembers.Remove(userId))
        {
            ConnectedMembersChanged?.Invoke();
        }
    }

    private void JoinGroupMemberAsync(string userId)
    {
        if (!connectedMembers.Contains(userId))
        {
            connectedMembers.Add(userId);
            ConnectedMembersChanged?.Invoke();
        }
    }

    private readonly List<string> connectedMembers = [];
    public IEnumerable<string> ConnectedMemberIds
    {
        get
        {
            foreach (var member in connectedMembers)
                yield return member;
        }
    }

    public bool IsConnected => connection.State == HubConnectionState.Connected;

    public async Task StartAsync()
    {
        await connection.StartAsync();

        await connection.SendAsync("JoinToProjectBoard", projectId, userIdentity);
    }

    public async Task SendUpdatedTaskAsync(int projectId, int taskId)
    {
        await SendWithAutoReconnect(() => connection.SendAsync(nameof(SendUpdatedTaskAsync), projectId, taskId));
    }

    public Task SendCreatedTaskAsync(int projectId, int taskId)
    {
        return SendWithAutoReconnect(() => connection.SendAsync(nameof(SendCreatedTaskAsync), projectId, taskId));
    }

    public Task SendDeletedTaskAsync(int projectId, int taskId)
    {
        return SendWithAutoReconnect(() => connection.SendAsync(nameof(SendDeletedTaskAsync), projectId, taskId));
    }

    private async Task SendWithAutoReconnect(Func<Task> action)
    {
        if (!IsConnected)
        {
            await StartAsync();
        }

        await action();
    }
    
    public event Action<OnlyofficeTask>? OnCreatedTask;
    public event Action<OnlyofficeTask>? OnUpdatedTask;
    public event Action<int>? OnDeletedTask;

    private async Task ReceiveUpdatedTaskAsync(int taskId)
    {
        var task = await taskClient.GetAsync(taskId);
        OnUpdatedTask?.Invoke(task);
    }

    private async Task ReceiveCreatedTaskAsync(int taskId)
    {
        var task = await taskClient.GetAsync(taskId);
        OnCreatedTask?.Invoke(task);
    }

    private Task ReceiveDeletedTaskAsync(int taskId)
    {
        OnDeletedTask?.Invoke(taskId);
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (connection is { State: HubConnectionState.Connected })
        {
            await connection.SendAsync("LeaveFromProjectBoard", projectId, userIdentity);
        }

        if (connection is { })
        {
            await connection.DisposeAsync();
        }
    }
}
