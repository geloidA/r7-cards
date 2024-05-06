using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api;

namespace Cardmngr.Application.Clients;

public interface IProjectClient
{
    Task<ProjectStateVm> GetProjectAsync(int projectId);
    IAsyncEnumerable<Project> GetSelfProjectsAsync();
    IAsyncEnumerable<Project> GetFollowedProjectsAsync();
    IAsyncEnumerable<ProjectStateVm> GetProjectsWithSelfTasksAsync();
    IAsyncEnumerable<Project> GetProjectsAsync();
    IAsyncEnumerable<KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>> GetGroupedFilteredTasksAsync(FilterBuilder filter);
    IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync(FilterBuilder filter);
    IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync();
    IAsyncEnumerable<OnlyofficeTaskStatus> GetTaskStatusesAsync();
    Task<ProjectStateVm> CreateProjectWithTasksAsync(ICollection<OnlyofficeTask> tasks);
    Task<Project> FollowProjectAsync(int projectId);
}
