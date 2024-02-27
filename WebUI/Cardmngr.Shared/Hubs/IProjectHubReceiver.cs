namespace Cardmngr.Shared.Hubs;

public interface IProjectHubReceiver
{
    Task ReceiveUpdatedTaskAsync(int taskId);
    Task ReceiveCreatedTaskAsync(int taskId);
    Task ReceiveDeletedTaskAsync(int taskId);

    Task JoinGroupMemberAsync(string userId);
    Task LeaveGroupMemberAsync(string userId);
}
