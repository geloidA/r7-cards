
namespace Cardmngr.Services;

public class AppProjectsInfoService : IProjectFollowChecker, IFollowedProjectManager
{
    private HashSet<int> followedProjectIds = [];

    public void Follow(int projectId) => followedProjectIds.Add(projectId);

    public bool IsFollow(int projectId) => followedProjectIds.Contains(projectId);

    public void Refresh(IEnumerable<int> followedProjectIds)
    {
        this.followedProjectIds = new HashSet<int>(followedProjectIds);
    }

    public void Unfollow(int projectId) => followedProjectIds.Remove(projectId);
}