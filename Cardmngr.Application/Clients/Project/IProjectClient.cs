using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients;

public interface IProjectClient
{
    Task<ProjectStateDto> GetProjectStateAsync(int projectId);
    Task<Project> GetProjectAsync(int projectId);
    Task<Project> DeleteProjectAsync(int projectId);
    IAsyncEnumerable<Project> GetSelfProjectsAsync();
    IAsyncEnumerable<Project> GetFollowedProjectsAsync();
    IAsyncEnumerable<ProjectStateDto> GetProjectsWithSelfTasksAsync();
    IAsyncEnumerable<Project> GetProjectsAsync();
    IAsyncEnumerable<KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>> GetGroupedFilteredTasksAsync(FilterBuilder filter);
    Task<ProjectStateDto> CollectProjectWithTasksAsync(ICollection<OnlyofficeTask> tasks);
    Task<Project> CreateProjectAsync(ProjectCreateDto project);
    Task<Project> FollowProjectAsync(int projectId);
}
