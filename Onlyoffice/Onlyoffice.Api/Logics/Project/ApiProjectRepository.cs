using System.Net.Http.Json;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Project;

public class ApiProjectRepository(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IProjectRepository
{
    public async Task<ProjectDto> FollowAsync(int id)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync<object>($"{ApiPaths.Project}/{id}/follow", null!));

        response.EnsureSuccessStatusCode();

        var projectDao = await response.Content.ReadFromJsonAsync<SingleProjectDao>();
        return projectDao?.Response ?? throw new NullReferenceException("Project was not followed");
    }

    public async Task<ProjectDto> DeleteAsync(int id)
    {
        var projectDao = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleProjectDao>($"{ApiPaths.Project}/{id}"));
        return projectDao?.Response ?? throw new NullReferenceException("Project was not deleted");
    }

    public async Task<ProjectDto> CreateAsync(ProjectCreateDto project)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync(ApiPaths.Project, project));

        response.EnsureSuccessStatusCode();

        var projectDao = await response.Content.ReadFromJsonAsync<SingleProjectDao>();
        return projectDao?.Response ?? throw new NullReferenceException("Project was not created");
    }

    public async IAsyncEnumerable<ProjectDto> GetAllFollowAsync()
    {
        var followProjectsDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectDao>($"{ApiPaths.Project}/@follow"));
        await foreach (var proj in followProjectsDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<ProjectDto>())
            yield return proj;
    }

    public async Task<ProjectDto> GetByIdAsync(int projectId)
    {
        var project = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<SingleProjectDao>($"{ApiPaths.Project}/{projectId}"));        
        return project?.Response ?? throw new NullReferenceException("Project was not found");
    }

    public async IAsyncEnumerable<ProjectDto> GetAllAsync()
    {
        var projectDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectDao>(ApiPaths.Project));
        await foreach (var project in projectDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<ProjectDto>())
            yield return project;
    }

    public async IAsyncEnumerable<ProjectInfoDto> GetUserProjectsAsync()
    {
        var projectDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<ProjectInfoDao>($"{ApiPaths.Project}/@self"));
        await foreach (var project in projectDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<ProjectInfoDto>())
            yield return project;
    }

    public async IAsyncEnumerable<UserProfileDto> GetTeamAsync(int id)
    {
        var projectTeamDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<UserProfilesDao>($"{ApiPaths.Project}/{id}/team"));
        await foreach (var userProfile in projectTeamDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<UserProfileDto>())
            yield return userProfile;
    }
}
