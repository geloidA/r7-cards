using Cardmngr.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Cardmngr.Server.Hubs;

public class ProjectBoardHub(GroupManager groupManager) : Hub<IProjectHubReceiver>, IProjectHubSender
{
    private readonly GroupManager groupManager = groupManager;
    private readonly Serilog.ILogger logger = Log.ForContext<ProjectBoardHub>();

    public string[] GetConnectedMembers(int projectId)
    {
        logger.Information("GetConnectedMembers. Connection: {connection}", Context.ConnectionId);
        return groupManager
            .GetUsersByProjectId(projectId)
            .ToArray();
    }

    public async Task JoinToProjectBoard(int projectId, string userId) // TODO: think about identity
    {
        logger.Information("JoinToProjectBoard. Context: {context}, ProjectId: {projectId}, UserId: {userId}", Context.ConnectionId, projectId, userId);
        await Groups.AddToGroupAsync(Context.ConnectionId, projectId.ToString());
        await Clients.OthersInGroup(projectId.ToString()).JoinGroupMemberAsync(userId);

        groupManager.Add(Context.ConnectionId, (userId, projectId));
    }

    public async Task LeaveFromProjectBoard(int projectId, string userId)
    {
        logger.Information("LeaveFromProjectBoard. Context: {context}, ProjectId: {projectId}, UserId: {userId}", Context.ConnectionId, projectId, userId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, projectId.ToString());
        await Clients.Group(projectId.ToString()).LeaveGroupMemberAsync(userId);

        groupManager.Remove(Context.ConnectionId);
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
        logger.Information("SendUpdatedTaskAsync. Connection: {connection}, ProjectId: {projectId}, TaskId: {taskId}", Context.ConnectionId, projectId, taskId);
        await Clients.OthersInGroup(projectId.ToString()).ReceiveUpdatedTaskAsync(taskId);
    }

    public async Task SendCreatedTaskAsync(int projectId, int taskId)
    {
        logger.Information("SendCreatedTaskAsync. Connection: {connection}, ProjectId: {projectId}, TaskId: {taskId}", Context.ConnectionId, projectId, taskId);
        await Clients.OthersInGroup(projectId.ToString()).ReceiveCreatedTaskAsync(taskId);
    }

    public async Task SendDeletedTaskAsync(int projectId, int taskId)
    {
        logger.Information("SendDeletedTaskAsync. Connection: {connection}, ProjectId: {projectId}, TaskId: {taskId}", Context.ConnectionId, projectId, taskId);
        await Clients.OthersInGroup(projectId.ToString()).ReceiveDeletedTaskAsync(taskId);
    }
}
