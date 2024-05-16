using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Repository;

public interface IProjectRepository
{
    IAsyncEnumerable<ProjectDto> GetAllAsync();
    Task<ProjectDto> GetByIdAsync(int id);
    IAsyncEnumerable<ProjectInfoDto> GetUserProjectsAsync();    
    Task<ProjectDto> FollowAsync(int id);
    IAsyncEnumerable<UserProfileDto> GetTeamAsync(int id);
    IAsyncEnumerable<ProjectDto> GetAllFollowAsync();
}
