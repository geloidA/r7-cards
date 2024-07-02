using AutoMapper;
using Cardmngr.Application.Extensions;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients;

// TODO: Separate logics
public class ProjectClient(
    IProjectRepository projectRepository, 
    ITaskRepository taskRepository, 
    ITaskStatusRepository taskStatusRepository,
    IMilestoneRepository milestoneRepository,
    IMapper mapper) : IProjectClient
{
    public async Task<ProjectStateDto> GetProjectStateAsync(int projectId)
    {
        var tasks = taskRepository
            .GetFiltredAsync(TaskFilterBuilder.Instance
                .ProjectId(projectId)
                .SortBy("updated")
                .SortOrder(FilterSortOrders.Desc))
            .ToListAsync(mapper.Map<OnlyofficeTask>);

        var team = projectRepository
            .GetTeamAsync(projectId)
            .ToListAsync(mapper.Map<UserProfile>);

        var milestones = milestoneRepository
            .GetAllByProjectIdAsync(projectId)
            .ToListAsync(mapper.Map<Domain.Entities.Milestone>);

        var project = projectRepository.GetByIdAsync(projectId);

        var statuses = taskStatusRepository
            .GetAllAsync()
            .ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        return new ProjectStateDto
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
        return projectRepository.GetAllAsync().Select(mapper.Map<Project>);
    }

    public async IAsyncEnumerable<ProjectStateDto> GetProjectsWithSelfTasksAsync()
    {
        var tasks = taskRepository.GetAllSelfAsync()
            .Select(mapper.Map<OnlyofficeTask>);
        var statuses = taskStatusRepository.GetAllAsync()
            .ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        await foreach (var tasksByProject in tasks.GroupBy(x => x.ProjectOwner.Id))
        {
            var team = projectRepository.GetTeamAsync(tasksByProject.Key)
                .ToListAsync(mapper.Map<UserProfile>);

            var milestones = milestoneRepository.GetAllByProjectIdAsync(tasksByProject.Key)
                                        .ToListAsync(mapper.Map<Domain.Entities.Milestone>);

            var project = projectRepository.GetByIdAsync(tasksByProject.Key);

            yield return new ProjectStateDto
            {
                Statuses = await statuses,
                Project = mapper.Map<Project>(await project),
                Milestones = await milestones,
                Team = await team,
                Tasks = await tasksByProject.ToListAsync()
            };
        }
    }

    public async IAsyncEnumerable<ProjectStateDto> GetFilteredTasksAsync(TaskFilterBuilder filter)
    {
        var statuses = taskStatusRepository
            .GetAllAsync()
            .ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        await foreach (var projectTasks in taskRepository.GetFiltredAsync(filter).GroupBy(x => x.ProjectOwner!.Id))
        {
            var project = projectRepository.GetByIdAsync(projectTasks.Key);

            var team = projectRepository
                .GetTeamAsync(projectTasks.Key)
                .ToListAsync(mapper.Map<UserProfile>);

            var milestones = milestoneRepository
                .GetAllByProjectIdAsync(projectTasks.Key)
                .ToListAsync(mapper.Map<Domain.Entities.Milestone>);

            yield return new ProjectStateDto
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
        await foreach (var project in taskRepository.GetFiltredAsync(filter).GroupBy(x => x.ProjectOwner))
        {
            yield return new KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>(
                mapper.Map<ProjectInfo>(project.Key), 
                mapper.Map<List<OnlyofficeTask>>(project));
        }
    }

    public IAsyncEnumerable<Project> GetSelfProjectsAsync()
    {
        return projectRepository.GetUserProjectsAsync().Select(mapper.Map<Project>);
    }

    public async Task<ProjectStateDto> CreateProjectWithTasksAsync(ICollection<OnlyofficeTask> tasks)
    {
        if (tasks.Count == 0)
        {
            throw new ArgumentException("Tasks collection cannot be empty");
        }
        
        var projectId = tasks.First().ProjectOwner.Id;

        if (tasks.Any(x => x.ProjectOwner.Id != projectId))
        {
            throw new ArgumentException("All tasks must be in the same project");
        }

        var statuses = taskStatusRepository
            .GetAllAsync()
            .ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        var team = projectRepository
            .GetTeamAsync(projectId)
            .ToListAsync(mapper.Map<UserProfile>);

        var project = projectRepository.GetByIdAsync(projectId);
        var milestones = milestoneRepository
            .GetAllByProjectIdAsync(projectId)
            .ToListAsync(mapper.Map<Domain.Entities.Milestone>);
        
        return new ProjectStateDto
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
        return mapper.Map<Project>(await projectRepository.FollowAsync(projectId));
    }

    public IAsyncEnumerable<Project> GetFollowedProjectsAsync()
    {
        return projectRepository
            .GetAllFollowAsync()
            .Select(mapper.Map<Project>);
    }

    public IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync(FilterBuilder filter)
    {
        return taskRepository
            .GetFiltredAsync(filter)
            .Select(mapper.Map<OnlyofficeTask>);
    }

    public IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync() => GetFilteredTasksAsync(FilterBuilder.Empty);

    public async Task<Project> GetProjectAsync(int projectId)
    {
        return mapper.Map<Project>(await projectRepository.GetByIdAsync(projectId));
    }
}