using AutoMapper;
using Cardmngr.Application.Clients;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Application;

public class ProjectClient(IProjectApi projectApi, IMapper mapper) : IProjectClient
{
    private readonly IProjectApi projectApi = projectApi;
    private readonly IMapper mapper = mapper;

    public async Task<ProjectStateVm> GetProjectAsync(int projectId)
    {
        var project = projectApi.GetProjectByIdAsync(projectId);
        var tasks = projectApi.GetFiltredTasksAsync(FilterTasksBuilder.Instance.WithProjectId(projectId)).ToListAsync();
        var statuses = projectApi.GetAllTaskStatusesAsync().ToListAsync();
        var milestones = projectApi.GetMilestonesByProjectIdAsync(projectId).ToListAsync();
        var team = projectApi.GetProjectTeamAsync(projectId).ToListAsync();

        return new ProjectStateVm
        {
            Project = mapper.Map<Project>(await project),
            Tasks = mapper.Map<List<OnlyofficeTask>>(await tasks),
            Statuses = mapper.Map<List<OnlyofficeTaskStatus>>(await statuses),
            Milestones = mapper.Map<List<Milestone>>(await milestones),
            Team = mapper.Map<List<UserProfile>>(await team)
        };
    }

    public async IAsyncEnumerable<Project> GetSelfProjectsAsync()
    {
        await foreach (var project in projectApi.GetUserProjectsAsync())
        {
            yield return mapper.Map<Project>(project);
        }
    }
}
