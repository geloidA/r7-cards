using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Repository;

public interface IProjectRepository : IRepository<ProjectDto>
{
    Task<ProjectDto> GetByIdAsync(int id);
    IAsyncEnumerable<ProjectInfoDto> GetUserProjectsAsync();    
    Task<ProjectDto> FollowAsync(int id);
    Task<ProjectDto> DeleteAsync(int id);
    Task<ProjectDto> CreateAsync(ProjectCreateDto project);
    IAsyncEnumerable<UserProfileDto> GetTeamAsync(int id);
    IAsyncEnumerable<ProjectDto> GetAllFollowAsync();
}
