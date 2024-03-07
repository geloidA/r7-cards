namespace Cardmngr.Server.Hubs;

public class GroupManager : ClientsManager<string, (string UserId, int ProjectId)>
{
    public IEnumerable<string> GetUsersByProjectId(int projectId)
    {
        return ValuePairs
            .Where(x => x.Value.ProjectId == projectId)
            .Select(x => x.Value.UserId);
    }
}
