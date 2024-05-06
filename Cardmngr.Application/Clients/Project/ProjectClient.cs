using AutoMapper;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Extensions;
using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api;
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
        var tasks = projectApi.GetFiltredTasksAsync(FilterTasksBuilder.Instance.ProjectId(projectId))
                              .ToListAsync(mapper.Map<OnlyofficeTask>);

        var team = projectApi.GetProjectTeamAsync(projectId)
                             .ToListAsync(mapper.Map<UserProfile>);

        var milestones = projectApi.GetMilestonesByProjectIdAsync(projectId)
                                   .ToListAsync(mapper.Map<Domain.Entities.Milestone>);

        var project = projectApi.GetProjectByIdAsync(projectId);

        var statuses = projectApi.GetAllTaskStatusesAsync()
                                 .ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        return new ProjectStateVm
        {
            Statuses = await statuses,
            Project = mapper.Map<Project>(await project),
            Milestones = await milestones,
            Team = await team,
            Tasks = await tasks
        };
    }

    public IAsyncEnumerable<Project> GetProjectsAsync()
    {
        return projectApi.GetProjectsAsync().Select(mapper.Map<Project>);
    }

    public async IAsyncEnumerable<ProjectStateVm> GetProjectsWithSelfTasksAsync()
    {
        var tasks = taskClient.GetSelfTasksAsync();
        var statuses = projectApi.GetAllTaskStatusesAsync()
                                 .ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        await foreach (var tasksByProject in tasks.GroupBy(x => x.ProjectOwner?.Id))
        {
            if (tasksByProject.Key != null)
            {
                var team = projectApi.GetProjectTeamAsync(tasksByProject.Key.Value).ToListAsync(mapper.Map<UserProfile>);

                var milestones = projectApi.GetMilestonesByProjectIdAsync(tasksByProject.Key.Value)
                                           .ToListAsync(mapper.Map<Domain.Entities.Milestone>);

                var project = projectApi.GetProjectByIdAsync(tasksByProject.Key.Value);

                yield return new ProjectStateVm
                {
                    Statuses = await statuses,
                    Project = mapper.Map<Project>(await project),
                    Milestones = await milestones,
                    Team = await team,
                    Tasks = await tasksByProject.ToListAsync()
                };
            }
            else
            {
                yield return new ProjectStateVm
                {
                    Statuses = await statuses,
                    Tasks = await tasksByProject.ToListAsync()
                };
            }
        }
    }

    public async IAsyncEnumerable<IProjectStateVm> GetFilteredTasksAsync(FilterTasksBuilder filter)
    {
        var statuses = projectApi.GetAllTaskStatusesAsync().ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        await foreach (var projectTasks in projectApi.GetFiltredTasksAsync(filter).GroupBy(x => x.ProjectOwner!.Id))
        {
            var project = projectApi.GetProjectByIdAsync(projectTasks.Key);
            var team = projectApi.GetProjectTeamAsync(projectTasks.Key).ToListAsync(mapper.Map<UserProfile>);
            var milestones = projectApi.GetMilestonesByProjectIdAsync(projectTasks.Key).ToListAsync(mapper.Map<Domain.Entities.Milestone>);

            yield return new ProjectStateVm
            {
                Project = mapper.Map<Project>(await project),
                Team = await team,
                Tasks = await projectTasks.ToListAsync(mapper.Map<OnlyofficeTask>),
                Milestones = await milestones,
                Statuses = await statuses
            };
        }
    }

    public async IAsyncEnumerable<KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>> GetGroupedFilteredTasksAsync(FilterBuilder filter)
    {
        await foreach (var project in projectApi.GetFiltredTasksAsync(filter).GroupBy(x => x.ProjectOwner))
        {
            yield return new KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>(
                mapper.Map<ProjectInfo>(project.Key), 
                mapper.Map<List<OnlyofficeTask>>(project));
        }
    }

    public IAsyncEnumerable<Project> GetSelfProjectsAsync()
    {
        return projectApi.GetUserProjectsAsync().Select(mapper.Map<Project>);
    }

    public async Task<ProjectStateVm> CreateProjectWithTasksAsync(ICollection<OnlyofficeTask> tasks)
    {
        var projectId = tasks.First().ProjectOwner.Id;

        if (tasks.Any(x => x.ProjectOwner.Id != projectId))
        {
            throw new ArgumentException("All tasks must be in the same project");
        }

        var statuses = projectApi.GetAllTaskStatusesAsync().ToListAsync(mapper.Map<OnlyofficeTaskStatus>);
        var team = projectApi.GetProjectTeamAsync(projectId).ToListAsync(mapper.Map<UserProfile>);
        var project = projectApi.GetProjectByIdAsync(projectId);
        var milestones = projectApi.GetMilestonesByProjectIdAsync(projectId).ToListAsync(mapper.Map<Domain.Entities.Milestone>);
        
        return new ProjectStateVm
        {
            Tasks = [.. tasks],
            Statuses = await statuses,
            Project = mapper.Map<Project>(await project),
            Milestones = await milestones,
            Team = await team
        };
    }

    public async Task<Project> FollowProjectAsync(int projectId)
    {
        return mapper.Map<Project>(await projectApi.FollowProjectAsync(projectId));
    }

    public IAsyncEnumerable<Project> GetFollowedProjectsAsync()
    {
        return projectApi.GetFollowProjectsAsync().Select(mapper.Map<Project>);
    }

    public IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync(FilterBuilder filter)
    {
        return projectApi.GetFiltredTasksAsync(filter).Select(mapper.Map<OnlyofficeTask>);
    }

    public IAsyncEnumerable<OnlyofficeTaskStatus> GetTaskStatusesAsync()
    {
        return projectApi.GetAllTaskStatusesAsync().Select(mapper.Map<OnlyofficeTaskStatus>);
    }

    public IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync() => GetFilteredTasksAsync(FilterBuilder.Empty);
}