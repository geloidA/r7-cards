namespace Cardmngr.Services;

public interface IFollowedProjectManager
{
    void Follow(int projectId);
    void Unfollow(int projectId);
    void Refresh(IEnumerable<int> followedProjectIds);
}
