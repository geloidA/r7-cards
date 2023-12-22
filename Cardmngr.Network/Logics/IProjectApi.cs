using Cardmngr.Network.Models;

namespace Cardmngr.Network.Logics;

public interface IProjectApi
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Projects that belongs to current user</returns>
    Task<IEnumerable<Project>> GetUserProjects();
    /// <summary>
    /// 
    /// </summary>
    /// <returns>All projects</returns>
    Task<IEnumerable<Project>> GetProjects();
    Task<Project> GetProjectById(int projectId);
}
