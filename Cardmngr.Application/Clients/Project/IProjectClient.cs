using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api.Common;

namespace Cardmngr.Application.Clients;

public interface IProjectClient
{
    Task<ProjectStateVm> GetProjectAsync(int projectId);
    IAsyncEnumerable<Project> GetSelfProjectsAsync();
    IAsyncEnumerable<ProjectStateVm> GetProjectsWithSelfTasksAsync();
    IAsyncEnumerable<Project> GetProjectsAsync();
    IAsyncEnumerable<IProjectStateVm> GetProjectsWithTaskFilterAsync(FilterTasksBuilder filter);
}
