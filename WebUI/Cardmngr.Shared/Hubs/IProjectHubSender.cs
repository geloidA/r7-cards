namespace Cardmngr.Shared.Hubs;

public interface IProjectHubSender
{
    Task SendUpdatedTaskAsync(int projectId, int taskId);
    Task SendCreatedTaskAsync(int projectId, int taskId);
    Task SendDeletedTaskAsync(int projectId, int taskId);
}
