using Cardmngr.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Cardmngr.Server.Hubs;

public class ProjectBoardHub(GroupManager groupManager) : Hub<IProjectHubReceiver>, IProjectHubSender
{
    private readonly GroupManager groupManager = groupManager;

    public async Task JoinToProjectBoard(int projectId, string userId) // TODO: think about identity
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, projectId.ToString());
        await Clients.OthersInGroup(projectId.ToString()).JoinGroupMemberAsync(userId);

        groupManager.Add(projectId, userId, Context.ConnectionId);
    }

    public async Task LeaveFromProjectBoard(int projectId, string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, projectId.ToString());
        await Clients.Group(projectId.ToString()).LeaveGroupMemberAsync(userId);

        groupManager.Remove(projectId, Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (groupManager.TryGet(Context.ConnectionId, out var data))
        {
            await LeaveFromProjectBoard(data.ProjectId, data.UserId);
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendUpdatedTaskAsync(int projectId, int taskId)
    {
        await Clients.OthersInGroup(projectId.ToString()).ReceiveUpdatedTaskAsync(taskId);
    }

    public async Task SendCreatedTaskAsync(int projectId, int taskId)
    {
        await Clients.OthersInGroup(projectId.ToString()).ReceiveCreatedTaskAsync(taskId);
    }

    public async Task SendDeletedTaskAsync(int projectId, int taskId)
    {
        await Clients.OthersInGroup(projectId.ToString()).ReceiveDeletedTaskAsync(taskId);
    }
}
