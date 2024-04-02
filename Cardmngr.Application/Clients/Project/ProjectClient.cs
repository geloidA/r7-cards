using AutoMapper;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Extensions;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Application.Clients;

public class ProjectClient(IProjectApi projectApi, ITaskClient taskClient, IMapper mapper) : IProjectClient
{
    private readonly IProjectApi projectApi = projectApi;
    private readonly ITaskClient taskClient = taskClient;
    private readonly IMapper mapper = mapper;

    public async Task<ProjectStateVm> GetProjectAsync(int projectId)
    {
        var project = projectApi.GetProjectByIdAsync(projectId);

        var tasks = projectApi.GetFiltredTasksAsync(FilterTasksBuilder.Instance.ProjectId(projectId))
            .ToListAsync(mapper.Map<OnlyofficeTask>);

        var statuses = projectApi.GetAllTaskStatusesAsync().ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        var milestones = projectApi.GetMilestonesByProjectIdAsync(projectId).ToListAsync(mapper.Map<Domain.Entities.Milestone>);

        var team = projectApi.GetProjectTeamAsync(projectId).ToListAsync(mapper.Map<UserProfile>);

        return new ProjectStateVm
        {
            Project = mapper.Map<Project>(await project),
            Tasks = await tasks,
            Statuses = await statuses,
            Milestones = await milestones,
            Team = await team
        };
    }

    public async IAsyncEnumerable<Project> GetProjectsAsync()
    {
        await foreach (var project in projectApi.GetProjectsAsync())
        {
            yield return mapper.Map<Project>(project);
        }
    }

    public async IAsyncEnumerable<ProjectStateVm> GetProjectsWithSelfTasksAsync()
    {
        var tasks = await taskClient.GetSelfTasksAsync().ToListAsync();
        var statuses = await projectApi.GetAllTaskStatusesAsync().ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        var tasksGroupByProject = tasks.GroupBy(x => x.ProjectOwner?.Id).ToAsyncEnumerable();

        await foreach (var tasksByProject in tasksGroupByProject)
        {
            if (tasksByProject.Key != null)
            {
                var team = projectApi.GetProjectTeamAsync(tasksByProject.Key.Value).ToListAsync();
                var milestones = projectApi.GetMilestonesByProjectIdAsync(tasksByProject.Key.Value).ToListAsync();
                var project = projectApi.GetProjectByIdAsync(tasksByProject.Key.Value);

                yield return new ProjectStateVm
                {
                    Project = mapper.Map<Project>(await project),
                    Milestones = mapper.Map<List<Domain.Entities.Milestone>>(await milestones),
                    Team = mapper.Map<List<UserProfile>>(await team),
                    Tasks = [.. tasksByProject],
                    Statuses = statuses
                };
            }
            else
            {
                yield return new ProjectStateVm
                {
                    Statuses = statuses,
                    Tasks = [.. tasksByProject]
                };
            }
        }
    }

    public async IAsyncEnumerable<IProjectStateVm> GetProjectsWithTaskFilterAsync(FilterTasksBuilder filter)
    {
        var statuses = await projectApi.GetAllTaskStatusesAsync().ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        await foreach (var projectTasks in projectApi.GetFiltredTasksAsync(filter).GroupBy(x => x.ProjectOwner!.Id))
        {
            var project = projectApi.GetProjectByIdAsync(projectTasks.Key);
            var team = projectApi.GetProjectTeamAsync(projectTasks.Key).ToListAsync(mapper.Map<UserProfile>);
            var milestones = projectApi.GetMilestonesByProjectIdAsync(projectTasks.Key).ToListAsync(mapper.Map<Domain.Entities.Milestone>);

            yield return new ProjectStateVm
            {
                Project = mapper.Map<Project>(await project),
                Team = await team,
                Tasks = mapper.Map<List<OnlyofficeTask>>(projectTasks),
                Milestones = await milestones,
                Statuses = statuses
            };
        }
    }

    public async IAsyncEnumerable<Project> GetSelfProjectsAsync()
    {
        await foreach (var project in projectApi.GetUserProjectsAsync())
        {
            yield return mapper.Map<Project>(project);
        }
    }
}
