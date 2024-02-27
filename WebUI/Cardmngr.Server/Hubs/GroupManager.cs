using System.Collections.Concurrent;

namespace Cardmngr.Server.Hubs;

public class GroupManager
{    
    private readonly ConcurrentDictionary<string, (string UserId, int ProjectId)> usersByConnectionId = [];

    public void Add(int projectId, string userId, string connectionId)
    {
        usersByConnectionId.TryAdd(connectionId, (userId, projectId));
    }

    public void Remove(int projectId, string connectionId)
    {
        if (usersByConnectionId.TryGetValue(connectionId, out var data))
        {
            usersByConnectionId.TryRemove(connectionId, out _);
        }
    }

    public bool TryGet(string connectionId, out (string UserId, int ProjectId) data)
    {
        return usersByConnectionId.TryGetValue(connectionId, out data);
    }
}
