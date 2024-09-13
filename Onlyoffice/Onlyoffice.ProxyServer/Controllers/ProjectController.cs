using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Onlyoffice.Api.Models;
using Onlyoffice.ProxyServer.Extensions;
using Task = System.Threading.Tasks.Task;

namespace Onlyoffice.ProxyServer.Controllers;

[Route("api/[controller]")]
public class ProjectController(IConfiguration conf) : ApiController(conf)
{
    #region Project Proxy
    [Route("")]
    public Task ProxyProject() => ProxyRequestAsync($"{ApiUrl}/project");

    [Route("{projectId:int}")]
    public Task ProxyProjectById(int projectId) => ProxyRequestAsync($"{ApiUrl}/project/{projectId}");

    [Route("@self")]
    public Task ProxyGetSelfProjects() => ProxyRequestAsync($"{ApiUrl}/project/@self");

    [Route("{projectId:int}/team")]
    public Task ProxyGetSelfProjectById(int projectId) => ProxyRequestAsync($"{ApiUrl}/project/{projectId}/team");

    [HttpPut("{projectId:int}/follow")]
    public Task ProxyFollowProject(int projectId) => ProxyRequestAsync($"{ApiUrl}/project/{projectId}/follow");
    #endregion

    #region Task Proxy
    [Route("status")]
    public Task ProxyTaskStatuses() => ProxyRequestAsync($"{ApiUrl}/project/status");

    [Route("{projectId:int}/task")]
    public Task ProxyTasksByProjectId(int projectId) => ProxyRequestAsync($"{ApiUrl}/project/{projectId}/task");

    [Route("task/{taskId:int}/status")]
    public Task ProxyUpdateTaskStatus(int taskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}/status");

    [Route("task/filter/{**rest}")]
    public Task ProxyFilterTasks(string rest) => ProxyRequestAsync($"{ApiUrl}/project/task/filter?{rest}");

    [Route("task/@self")]
    public Task ProxyGetSelfTasks() => ProxyRequestAsync($"{ApiUrl}/project/task/@self");

    [HttpGet("@follow")]
    public Task ProxyGetFollowedProjects() => ProxyRequestAsync($"{ApiUrl}/project/@follow");

    [HttpPost("{projectId:int}/task")]
    public Task ProxyCreateTask(int projectId) => ProxyRequestAsync($"{ApiUrl}/project/{projectId}/task");

    [HttpDelete("task/{taskId:int}")]
    public Task ProxyDeleteTask(int taskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}");

    [HttpPut("task/{taskId:int}")]
    public Task ProxyUpdateTask(int taskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}");

    [HttpGet("task/{taskId:int}")]
    public Task ProxyTaskById(int taskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}");

    [HttpGet("task/{taskId:int}/comment")]
    public Task ProxyGetTaskComments(int taskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}/comment");

    [HttpPost("task/{taskId:int}/comment")]
    public Task ProxyCreateTaskComment(int taskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}/comment");

    [HttpDelete("comment/{commentId}")]
    public Task ProxyDeleteTaskComment(string commentId) => ProxyRequestAsync($"{ApiUrl}/project/comment/{commentId}");

    
    #endregion

    #region Subtask Proxy
    
    [HttpPut("task/{taskId:int}/{subtaskId:int}")]
    public Task ProxyUpdateSubtask(int taskId, int subtaskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}/{subtaskId}");

    [HttpPut("task/{taskId:int}/{subtaskId:int}/status")]
    public Task ProxyUpdateSubtaskStatus(int taskId, int subtaskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}/{subtaskId}/status");

    [HttpDelete("task/{taskId:int}/{subtaskId:int}")]
    public Task ProxyDeleteSubtask(int taskId, int subtaskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}/{subtaskId}");

    [HttpPost("task/{taskId:int}")]
    public Task ProxyCreateSubtask(int taskId) => ProxyRequestAsync($"{ApiUrl}/project/task/{taskId}");
    #endregion

    #region Milestone Proxy
    [HttpGet("{projectId:int}/milestone")]
    public Task ProxyMilestones(int projectId) => ProxyRequestAsync($"{ApiUrl}/project/{projectId}/milestone");

    [HttpGet("milestone/{milestoneId:int}")]
    public Task ProxyMilestoneById(int milestoneId) => ProxyRequestAsync($"{ApiUrl}/project/milestone/{milestoneId}");

    [HttpPost("{projectId:int}/milestone")]
    public Task ProxyCreateMilestone(int projectId) => ProxyRequestAsync($"{ApiUrl}/project/{projectId}/milestone");

    [HttpDelete("milestone/{milestoneId:int}")]
    public Task ProxyDeleteMilestone(int milestoneId) => ProxyRequestAsync($"{ApiUrl}/project/milestone/{milestoneId}");

    [HttpPut("milestone/{milestoneId:int}/status")]
    public Task ProxyUpdateMilestoneStatus(int milestoneId) => ProxyRequestAsync($"{ApiUrl}/project/milestone/{milestoneId}/status");

    [HttpPut("milestone/{milestoneId:int}")]
    public Task ProxyUpdateMilestone(int milestoneId) => ProxyRequestAsync($"{ApiUrl}/project/milestone/{milestoneId}");
    #endregion

    [HttpPost("{projectId:int}/task/status")]
    public async Task CreateTaskWithStatus(int projectId, [FromBody] TaskUpdateData updateData)
    {
        var authCookie = HttpContext.Request.Cookies;

        var task = await CreateTaskAsync(projectId, updateData, authCookie);
        var response = await UpdateTaskStatus(task.Id, updateData.Status, updateData.TaskStatusId, authCookie);
        
        await response.Content.CopyToAsync(HttpContext.Response.Body);
    }

    private async Task<TaskDto> CreateTaskAsync(int projectId, TaskUpdateData state, IRequestCookieCollection cookie)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiUrl}/project/{projectId}/task")
        {
            Content = new StringContent(JsonSerializer.Serialize(state), Encoding.UTF8, "application/json")
        };

        using var client = cookie.GetClientFor(ApiUrl, "asc_auth_key");
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao!.Response!;
    }

    private async Task<HttpResponseMessage> UpdateTaskStatus(int id, int? status, int? statusId, IRequestCookieCollection cookie)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{ApiUrl}/project/task/{id}/status")
        {
            Content = new StringContent(JsonSerializer.Serialize(new { status, statusId }), Encoding.UTF8, "application/json")
        };
        
        using var client = cookie.GetClientFor(ApiUrl, "asc_auth_key");
        return await client.SendAsync(request);
    }
}