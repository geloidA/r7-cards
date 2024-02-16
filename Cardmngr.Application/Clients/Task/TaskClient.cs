using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Application.Clients;

public class TaskClient(IProjectApi projectApi, IMapper mapper) : ITaskClient
{
    private readonly IProjectApi projectApi = projectApi;
    private readonly IMapper mapper = mapper;

    public async Task<OnlyofficeTask> CreateTaskAsync(int projectId, OnlyofficeTask task)
    {
        var createdTask = await projectApi.CreateTaskAsync(projectId, 
                mapper.Map<Onlyoffice.Api.Models.UpdatedStateTask>(task), 
                mapper.Map<Status>(task.Status), 
                task.TaskStatusId);

        if (task.Subtasks.Count != 0)
        {
            var createSubtaskTasks = task.Subtasks
                .Select(subtask => projectApi.CreateSubtaskAsync(createdTask.Id, subtask.Title, subtask.Responsible.Id));

            await Task.WhenAll(createSubtaskTasks);

            createdTask = await projectApi.GetTaskByIdAsync(createdTask.Id);
        }

        return mapper.Map<OnlyofficeTask>(createdTask);    
    }

    public Task RemoveTaskAsync(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<OnlyofficeTask> UpdateTaskAsync(int projectId, OnlyofficeTask task)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTaskStatusAsync(int taskId, OnlyofficeTaskStatus status)
    {
        await projectApi.UpdateTaskStatusAsync(taskId, mapper.Map<Status>(status.StatusType), status.Id);
    }
}
