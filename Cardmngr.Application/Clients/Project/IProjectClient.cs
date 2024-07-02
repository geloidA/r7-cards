using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients;

public interface IProjectClient
{
    Task<ProjectStateDto> GetProjectStateAsync(int projectId);
    Task<Project> GetProjectAsync(int projectId);
    IAsyncEnumerable<Project> GetSelfProjectsAsync();
    IAsyncEnumerable<Project> GetFollowedProjectsAsync();
    IAsyncEnumerable<ProjectStateDto> GetProjectsWithSelfTasksAsync();
    IAsyncEnumerable<Project> GetProjectsAsync();
    IAsyncEnumerable<KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>> GetGroupedFilteredTasksAsync(FilterBuilder filter);
    Task<ProjectStateDto> CreateProjectWithTasksAsync(ICollection<OnlyofficeTask> tasks);
    Task<Project> FollowProjectAsync(int projectId);
}
