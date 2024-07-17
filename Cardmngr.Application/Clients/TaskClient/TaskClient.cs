using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;
using Cardmngr.Domain.Extensions;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.TaskClient;

public class TaskClient(ITaskRepository taskRepository, IMapper mapper) : ITaskClient
{
    public async Task<OnlyofficeTask> CreateAsync(int projectId, TaskUpdateData task)
    {
        return mapper.Map<OnlyofficeTask>(await taskRepository.CreateAsync(projectId, task));
    }

    public async Task<TaskTag> CreateTagAsync(int taskId, string name)
    {
        var commentDto = await taskRepository.CreateCommentAsync(taskId, new CommentUpdateData { Content = name.ToCommentContent() });
        return new TaskTag(commentDto.Id, name, commentDto.CanEdit);
    }

    public IAsyncEnumerable<OnlyofficeTask> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<OnlyofficeTask> GetAsync(int entityId)
    {
        return mapper.Map<OnlyofficeTask>(await taskRepository.GetByIdAsync(entityId));
    }

    public IAsyncEnumerable<OnlyofficeTask> GetEntitiesAsync(FilterBuilder? filterBuilder = null)
    {
        return taskRepository
            .GetFiltredAsync(filterBuilder ?? FilterBuilder.Empty)
            .Select(mapper.Map<OnlyofficeTask>);
    }

    public IAsyncEnumerable<OnlyofficeTask> GetSelfTasksAsync()
    {
        return taskRepository
            .GetAllSelfAsync()
            .Select(mapper.Map<OnlyofficeTask>);
    }

    public IAsyncEnumerable<Comment> GetTaskCommentsAsync(int taskId)
    {
        return taskRepository.GetCommentsAsync(taskId)
            .Select(mapper.Map<Comment>);
    }

    public async IAsyncEnumerable<TaskTag> GetTaskTagsAsync(int taskId)
    {
        await foreach (var comment in taskRepository.GetCommentsAsync(taskId))
        {
            if (comment.Text.TryParseToTagName(out var tagName))
                yield return new TaskTag(comment.Id, tagName, comment.CanEdit);
        }
    }

    public async Task<OnlyofficeTask> RemoveAsync(int taskId)
    {
        return mapper.Map<OnlyofficeTask>(await taskRepository.DeleteAsync(taskId));
    }

    public Task RemoveTagAsync(string tagId) => taskRepository.RemoveCommentAsync(tagId);

    public async Task<OnlyofficeTask> UpdateAsync(int taskId, TaskUpdateData task)
    {
        return mapper.Map<OnlyofficeTask>(await taskRepository.UpdateAsync(taskId, task));
    }

    public async Task<OnlyofficeTask> UpdateTaskStatusAsync(int taskId, OnlyofficeTaskStatus status)
    {
        var taskDto = await taskRepository.UpdateStatusAsync(taskId, mapper.Map<Status>(status.StatusType), status.Id);
        return mapper.Map<OnlyofficeTask>(taskDto);
    }
}
