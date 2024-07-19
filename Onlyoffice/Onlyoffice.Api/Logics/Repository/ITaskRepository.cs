using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Onlyoffice.Api.Logics.Repository;

public interface ITaskRepository
{
    IAsyncEnumerable<TaskDto> GetAllByProjectIdAsync(int projectId);
    Task<TaskDto> GetByIdAsync(int id);
    Task<CommentDto> CreateCommentAsync(int id, CommentUpdateData comment);
    Task RemoveCommentAsync(string commentId);
    IAsyncEnumerable<CommentDto> GetCommentsAsync(int id);
    IAsyncEnumerable<TaskDto> GetFilteredAsync(FilterBuilder builder);
    Task<TaskDto> CreateAsync(int projectId, TaskUpdateData updateData);
    IAsyncEnumerable<TaskDto> GetAllSelfAsync();
    Task<TaskDto> UpdateAsync(int id, TaskUpdateData state);
    Task<TaskDto> UpdateStatusAsync(int id, Status status, int? statusId = null);
    Task<TaskDto> DeleteAsync(int id);
}
