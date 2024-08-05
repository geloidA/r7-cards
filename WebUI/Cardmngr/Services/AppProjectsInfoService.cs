
namespace Cardmngr.Services;

public class AppProjectsInfoService : IProjectFollowChecker, IFollowedProjectManager
{
    private HashSet<int> _followedProjectIds = [];

    public void Follow(int projectId) => _followedProjectIds.Add(projectId);

    public bool IsFollow(int projectId) => _followedProjectIds.Contains(projectId);

    public void Refresh(IEnumerable<int> followedProjectIds)
    {
        _followedProjectIds = [..followedProjectIds];
    }

    public void Unfollow(int projectId) => _followedProjectIds.Remove(projectId);
}