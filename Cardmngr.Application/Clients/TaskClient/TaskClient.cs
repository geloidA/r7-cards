using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Models;
using Cardmngr.Domain.Extensions;

namespace Cardmngr.Application.Clients.TaskClient;

public class TaskClient(IProjectApi projectApi, IMapper mapper) : ITaskClient
{
    private readonly IProjectApi projectApi = projectApi;
    private readonly IMapper mapper = mapper;

    public async Task<OnlyofficeTask> CreateAsync(int projectId, TaskUpdateData task)
    {
        var createdTask = await projectApi.CreateTaskAsync(projectId, task, (Status)task.Status!, task.CustomTaskStatus);
        return mapper.Map<OnlyofficeTask>(createdTask);
    }

    public async Task<TaskTag> CreateTagAsync(int taskId, string name)
    {
        var commentDto = await projectApi.CreateTaskCommentAsync(taskId, new CommentUpdateData { Content = name.ToCommentContent() });
        return new TaskTag(commentDto.Id, name, commentDto.CanEdit);
    }

    public async Task<OnlyofficeTask> GetAsync(int entityId)
    {
        var taskDto = await projectApi.GetTaskByIdAsync(entityId);
        return mapper.Map<OnlyofficeTask>(taskDto);
    }

    public async IAsyncEnumerable<OnlyofficeTask> GetSelfTasksAsync()
    {
        await foreach (var tasksDto in projectApi.GetSelfTasksAsync())
        {
            yield return mapper.Map<OnlyofficeTask>(tasksDto);
        }
    }

    public async IAsyncEnumerable<Comment> GetTaskCommentsAsync(int taskId)
    {
        await foreach (var comment in projectApi.GetTaskCommentsAsync(taskId))
        {
            yield return mapper.Map<Comment>(comment);
        }
    }

    public async IAsyncEnumerable<TaskTag> GetTaskTagsAsync(int taskId)
    {
        await foreach (var comment in projectApi.GetTaskCommentsAsync(taskId))
        {
            if (comment.Text.TryParseToTagName(out var tagName))
            {
                yield return new TaskTag(comment.Id, tagName, comment.CanEdit);
            }
        }
        yield break;
    }

    public async Task<OnlyofficeTask> RemoveAsync(int taskId)
    {
        var taskDto = await projectApi.DeleteTaskAsync(taskId);
        return mapper.Map<OnlyofficeTask>(taskDto);
    }

    public Task RemoveTagAsync(string tagId) => projectApi.RemoveTaskCommentAsync(tagId);

    public async Task<OnlyofficeTask> UpdateAsync(int taskId, TaskUpdateData task)
    {
        var taskDto = await projectApi.UpdateTaskAsync(taskId, task);
        return mapper.Map<OnlyofficeTask>(taskDto);
    }

    public async Task<OnlyofficeTask> UpdateTaskStatusAsync(int taskId, OnlyofficeTaskStatus status)
    {
        var taskDto = await projectApi.UpdateTaskStatusAsync(taskId, mapper.Map<Status>(status.StatusType), status.Id);
        return mapper.Map<OnlyofficeTask>(taskDto);
    }
}
