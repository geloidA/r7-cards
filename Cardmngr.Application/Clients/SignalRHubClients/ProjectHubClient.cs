using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Onlyoffice.Api.Providers;

namespace Cardmngr.Application.Clients.SignalRHubClients;

public sealed class ProjectHubClient : IAsyncDisposable
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
            .WithUrl(navigationManager.ToAbsoluteUri(HubPatterns.ProjectBoard))
            .Build();

        RegisterHandlers(connection);

        userIdentity = authStateProvider.UserId;

        this.projectId = projectId;
        this.taskClient = taskClient;
    }

    private void RegisterHandlers(HubConnection connection)
    {
        connection.On<int>(nameof(ReceiveCreatedTaskAsync), ReceiveCreatedTaskAsync);
        connection.On<int>(nameof(ReceiveUpdatedTaskAsync), ReceiveUpdatedTaskAsync);
        connection.On<int>("ReceiveDeletedTaskAsync", ReceiveDeletedTask);

        connection.On<string>(nameof(JoinGroupMemberAsync), JoinGroupMemberAsync);
        connection.On<string>(nameof(LeaveGroupMemberAsync), LeaveGroupMemberAsync);
    }

    public event Action<MembersChangedEventArgs>? ConnectedMembersChanged;

    private void OnMembersChanged(MemberAction action, string userId) 
        => ConnectedMembersChanged?.Invoke(new MembersChangedEventArgs(action, userId));

    private void LeaveGroupMemberAsync(string userId)
    {
        if (connectedMembers.Remove(userId))
        {
            OnMembersChanged(MemberAction.Leave, userId);
        }
    }

    private void JoinGroupMemberAsync(string userId)
    {
        if (connectedMembers.Add(userId))
        {
            OnMembersChanged(MemberAction.Join, userId);
        }
    }

    private HashSet<string> connectedMembers = [];
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

        var members = await connection.InvokeAsync<string[]>("GetConnectedMembers", projectId);
        connectedMembers = [.. members.Where(x => x != userIdentity)];
    }

    public Task SendUpdatedTaskAsync(int projectId, int taskId)
    {
        return SendWithAutoReconnect(() => connection.SendAsync(nameof(SendUpdatedTaskAsync), projectId, taskId));
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

    #region Handlers
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

    private void ReceiveDeletedTask(int taskId) => OnDeletedTask?.Invoke(taskId);

    #endregion

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
