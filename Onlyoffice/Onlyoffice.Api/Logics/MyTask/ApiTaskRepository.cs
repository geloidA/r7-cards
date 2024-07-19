using System.Net.Http.Json;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Onlyoffice.Api.Logics.MyTask;

public class ApiTaskRepository(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), ITaskRepository
{
    public async Task<TaskDto> CreateAsync(int projectId, TaskUpdateData updateData)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"{ApiPaths.Project}/{projectId}/task/status", updateData));

        response.EnsureSuccessStatusCode();

        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not created");
    }

    public async Task<CommentDto> CreateCommentAsync(int id, CommentUpdateData comment)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"{ApiPaths.Task}/{id}/comment", comment));

        response.EnsureSuccessStatusCode();
        var commentsDao = await response.Content.ReadFromJsonAsync<SingleCommentDao>();
        return commentsDao?.Response ?? throw new NullReferenceException("Task was not created");
    }

    public async Task<TaskDto> DeleteAsync(int id)
    {
        var deletedTask = await InvokeHttpClientAsync(c => c.DeleteFromJsonAsync<SingleTaskDao>($"{ApiPaths.Task}/{id}"));
        return deletedTask?.Response ?? throw new NullReferenceException("Task was not deleted");
    }

    public async IAsyncEnumerable<TaskDto> GetAllByProjectIdAsync(int projectId)
    {
        var taskDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskDao>($"{ApiPaths.Project}/{projectId}/task"));
        await foreach (var task in taskDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TaskDto>())
            yield return task;
    }

    public async IAsyncEnumerable<TaskDto> GetAllSelfAsync()
    {
        var tasksDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<TaskDao>($"{ApiPaths.Task}/@self"));
        await foreach (var task in tasksDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TaskDto>())
            yield return task;
    }

    public async Task<TaskDto> GetByIdAsync(int id)
    {
        var taskDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<SingleTaskDao>($"{ApiPaths.Task}/{id}"));
        return taskDao?.Response ?? throw new NullReferenceException("Task was not found");
    }

    public async IAsyncEnumerable<CommentDto> GetCommentsAsync(int id)
    {
        var response = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<CommentsDao>($"{ApiPaths.Task}/{id}/comment"));        
        foreach (var comment in response?.Response.Where(x => !x.Inactive) ?? [])
            yield return comment;
    }

    public async IAsyncEnumerable<TaskDto> GetFilteredAsync(FilterBuilder builder)
    {
        var filterTasksDao = await InvokeHttpClientAsync(c => c.GetFromJsonAsync<FilterTasksDao>($"{ApiPaths.Task}/filter/{builder.Build()}"));
        await foreach (var task in filterTasksDao?.Response?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TaskDto>())
            yield return task;
    }

    public async Task RemoveCommentAsync(string commentId) //TODO: move to repository
    {
        var response = await InvokeHttpClientAsync(c => c.DeleteAsync($"api/project/comment/{commentId}"));
        response.EnsureSuccessStatusCode();
    }

    public async Task<TaskDto> UpdateAsync(int id, TaskUpdateData state)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"{ApiPaths.Task}/{id}", state));
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not updated");
    }

    public async Task<TaskDto> UpdateStatusAsync(int id, Status status, int? statusId = null)
    {
        var response = await InvokeHttpClientAsync(c => c.PutAsJsonAsync($"{ApiPaths.Task}/{id}/status", new { status, statusId }));
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not updated");
    }
}
