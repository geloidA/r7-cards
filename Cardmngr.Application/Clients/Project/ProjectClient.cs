﻿using AutoMapper;
using Cardmngr.Application.Extensions;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Cardmngr.Shared.Utils;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.Project;

// TODO: Separate logics
public class ProjectClient(IProjectRepository projectRepository,
    ITaskRepository taskRepository,
    ITaskStatusRepository taskStatusRepository,
    IMilestoneRepository milestoneRepository,
    IMapper mapper) : IProjectClient
{
    public async Task<ProjectStateDto> GetProjectStateAsync(int projectId)
    {
        var tasks = taskRepository
            .GetFilteredAsync(TaskFilterBuilder.Instance
                .ProjectId(projectId)
                .SortBy("updated")
                .SortOrder(FilterSortOrders.Desc))
            .ToListAsync(mapper.Map<OnlyofficeTask>);

        var team = Catcher.CatchAsync<HttpRequestException, List<UserProfile>>(
            () => projectRepository.GetTeamAsync(projectId).ToListAsync(mapper.Map<UserProfile>),
            defaultValue: []);

        var milestones = Catcher.CatchAsync<HttpRequestException, List<Domain.Entities.Milestone>>(
            () => milestoneRepository.GetAllByProjectIdAsync(projectId).ToListAsync(mapper.Map<Domain.Entities.Milestone>),
            defaultValue: []);

        var project = projectRepository.GetByIdAsync(projectId);

        var statuses = taskStatusRepository
            .GetAllAsync()
            .ToListAsync(mapper.Map<OnlyofficeTaskStatus>);

        return new ProjectStateDto
        {
            Statuses = await statuses,
            Project = mapper.Map<Domain.Entities.Project>(await project),
            Milestones = await milestones,
            Team = await team,
            Tasks = await tasks
        };
    }

    public IAsyncEnumerable<Domain.Entities.Project> GetProjectsAsync()
    {
        return projectRepository.GetAllAsync().Select(mapper.Map<Domain.Entities.Project>);
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
                Project = mapper.Map<Domain.Entities.Project>(await project),
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

        await foreach (var projectTasks in taskRepository.GetFilteredAsync(filter).GroupBy(x => x.ProjectOwner!.Id))
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
                Project = mapper.Map<Domain.Entities.Project>(await project),
                Team = await team,
                Tasks = await projectTasks.ToListAsync(mapper.Map<OnlyofficeTask>),
                Milestones = await milestones,
                Statuses = await statuses
            };
        }
    }

    public IAsyncEnumerable<KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>> GetGroupedFilteredTasksAsync(FilterBuilder filter)
    {
        return taskRepository.GetFilteredAsync(filter)
            .GroupBy(x => x.ProjectOwner)
            .Select(project => new KeyValuePair<ProjectInfo, ICollection<OnlyofficeTask>>(
            mapper.Map<ProjectInfo>(project.Key),
            mapper.Map<List<OnlyofficeTask>>(project)));
    }

    public async Task<Domain.Entities.Project> DeleteProjectAsync(int projectId)
    {
        var project = await projectRepository.DeleteAsync(projectId);
        return mapper.Map<Domain.Entities.Project>(project);
    }

    public IAsyncEnumerable<Domain.Entities.Project> GetSelfProjectsAsync()
    {
        return projectRepository.GetUserProjectsAsync().Select(mapper.Map<Domain.Entities.Project>);
    }

    public async Task<ProjectStateDto> CollectProjectWithTasksAsync(ICollection<OnlyofficeTask> tasks, List<OnlyofficeTaskStatus>? statuses = null)
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

        var resultStatuses = await Catcher.CatchAsync<HttpRequestException, List<OnlyofficeTaskStatus>>(
            () => statuses is null ? taskStatusRepository.GetAllAsync().ToListAsync(mapper.Map<OnlyofficeTaskStatus>) : ValueTask.FromResult(statuses), defaultValue: []);

        var team = await Catcher.CatchAsync<HttpRequestException, List<UserProfile>>(
            () => projectRepository.GetTeamAsync(projectId).ToListAsync(mapper.Map<UserProfile>), defaultValue: []);

        var project = projectRepository.GetByIdAsync(projectId);
        
        var milestones = await Catcher.CatchAsync<HttpRequestException, List<Domain.Entities.Milestone>>(
            () => milestoneRepository.GetAllByProjectIdAsync(projectId).ToListAsync(mapper.Map<Domain.Entities.Milestone>),
            defaultValue: []);

        return new ProjectStateDto
        {
            Tasks = [.. tasks],
            Statuses = resultStatuses,
            Project = mapper.Map<Domain.Entities.Project>(await project),
            Milestones = milestones,
            Team = team
        };
    }

    public async Task<Domain.Entities.Project> CreateProjectAsync(ProjectCreateDto project)
    {
        var created = await projectRepository.CreateAsync(project);
        return mapper.Map<Domain.Entities.Project>(created);
    }

    public async Task<Domain.Entities.Project> FollowProjectAsync(int projectId)
    {
        return mapper.Map<Domain.Entities.Project>(await projectRepository.FollowAsync(projectId));
    }

    public IAsyncEnumerable<Domain.Entities.Project> GetFollowedProjectsAsync()
    {
        return projectRepository
            .GetAllFollowAsync()
            .Select(mapper.Map<Domain.Entities.Project>);
    }

    private IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync(FilterBuilder filter)
    {
        return taskRepository
            .GetFilteredAsync(filter)
            .Select(mapper.Map<OnlyofficeTask>);
    }

    public IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync() => GetFilteredTasksAsync(FilterBuilder.Empty);

    public async Task<Domain.Entities.Project> GetProjectAsync(int projectId)
    {
        return mapper.Map<Domain.Entities.Project>(await projectRepository.GetByIdAsync(projectId));
    }
}
