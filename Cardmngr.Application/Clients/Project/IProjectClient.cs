using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;

namespace Cardmngr.Application.Clients;

public interface IProjectClient
{
    Task<ProjectStateVm> GetProjectAsync(int projectId);
    IAsyncEnumerable<Project> GetSelfProjectsAsync();
}
