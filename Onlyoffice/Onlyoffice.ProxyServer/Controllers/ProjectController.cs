using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Onlyoffice.Api.Models;
using Onlyoffice.ProxyServer.Extensions;
using Task = System.Threading.Tasks.Task;

namespace Onlyoffice.ProxyServer.Controllers;

public class ProjectController(IConfiguration conf) : ApiController(conf)
{
    #region Project Proxy
    [Route("api/[controller]")]
    public Task ProxyProject() => ProxyRequestAsync($"{apiUrl}/project");

    [Route("api/[controller]/{projectId}")]
    public Task ProxyProjectById(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}");

    [Route("api/[controller]/@self")]
    public Task ProxyGetSelfProjects() => ProxyRequestAsync($"{apiUrl}/project/@self");

    [Route("api/[controller]/{projectId}/team")]
    public Task ProxyGetSelfProjectById(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/team");

    [HttpPut("api/[controller]/{projectId}/follow")]
    public Task ProxyFollowProject(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/follow");
    #endregion

    #region Task Proxy
    [Route("api/[controller]/status")]
    public Task ProxyTaskStatuses() => ProxyRequestAsync($"{apiUrl}/project/status");

    [Route("api/[controller]/{projectId}/task")]
    public Task ProxyTasksByProjectId(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/task");

    [Route("api/[controller]/task/{taskId}/status")]
    public Task ProxyUpdateTaskStatus(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/status");

    [Route("api/[controller]/task/filter/{**rest}")]
    public Task ProxyFilterTasks(string rest) => ProxyRequestAsync($"{apiUrl}/project/task/filter?{rest}");

    [Route("api/[controller]/task/@self")]
    public Task ProxyGetSelfTasks() => ProxyRequestAsync($"{apiUrl}/project/task/@self");

    [HttpGet("api/[controller]/@follow")]
    public Task ProxyGetFollowedProjects() => ProxyRequestAsync($"{apiUrl}/project/@follow");

    [HttpPost("api/[controller]/{projectId}/task")]
    public Task ProxyCreateTask(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/task");

    [HttpDelete("api/[controller]/task/{taskId}")]
    public Task ProxyDeleteTask(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}");

    [HttpPut("api/[controller]/task/{taskId}")]
    public Task ProxyUpdateTask(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}");

    [HttpGet("api/[controller]/task/{taskId}")]
    public Task ProxyTaskById(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}");

    [HttpGet("api/[controller]/task/{taskId}/comment")]
    public Task ProxyGetTaskComments(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/comment");

    [HttpPost("api/[controller]/task/{taskId}/comment")]
    public Task ProxyCreateTaskComment(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/comment");

    [HttpDelete("api/[controller]/comment/{commentId}")]
    public Task ProxyDeleteTaskComment(string commentId) => ProxyRequestAsync($"{apiUrl}/project/comment/{commentId}");

    
    #endregion

    #region Subtask Proxy
    [HttpPut("api/[controller]/task/{taskId}/{subtaskId}")]
    public Task ProxyUpdateSubtask(int taskId, int subtaskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/{subtaskId}");

    [HttpPut("api/[controller]/task/{taskId}/{subtaskId}/status")]
    public Task ProxyUpdateSubtaskStatus(int taskId, int subtaskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/{subtaskId}/status");

    [HttpDelete("api/[controller]/task/{taskId}/{subtaskId}")]
    public Task ProxyDeleteSubtask(int taskId, int subtaskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/{subtaskId}");

    [HttpPost("api/[controller]/task/{taskId}")]
    public Task ProxyCreateSubtask(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}");
    #endregion

    #region Milestone Proxy
    [HttpGet("api/[controller]/{projectId}/milestone")]
    public Task ProxyMilestones(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/milestone");

    [HttpGet("api/[controller]/milestone/{milestoneId}")]
    public Task ProxyMilestoneById(int milestoneId) => ProxyRequestAsync($"{apiUrl}/project/milestone/{milestoneId}");

    [HttpPost("api/[controller]/{projectId}/milestone")]
    public Task ProxyCreateMilestone(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/milestone");

    [HttpDelete("api/[controller]/milestone/{milestoneId}")]
    public Task ProxyDeleteMilestone(int milestoneId) => ProxyRequestAsync($"{apiUrl}/project/milestone/{milestoneId}");

    [HttpPut("api/[controller]/milestone/{milestoneId}/status")]
    public Task ProxyUpdateMilestoneStatus(int milestoneId) => ProxyRequestAsync($"{apiUrl}/project/milestone/{milestoneId}/status");

    [HttpPut("api/[controller]/milestone/{milestoneId}")]
    public Task ProxyUpdateMilestone(int milestoneId) => ProxyRequestAsync($"{apiUrl}/project/milestone/{milestoneId}");
    #endregion

    [HttpPost("api/[controller]/{projectId}/task/status")]
    public async Task CreateTaskWithStatus(int projectId, [FromBody] TaskUpdateData updateData)
    {
        var authCookie = HttpContext.Request.Cookies;

        var task = await CreateTaskAsync(projectId, updateData, authCookie);
        var response = await UpdateTaskStatus(task.Id, updateData.Status, updateData.CustomTaskStatus, authCookie);
        
        await response.Content.CopyToAsync(HttpContext.Response.Body);
    }

    private async Task<TaskDto> CreateTaskAsync(int projectId, TaskUpdateData state, IRequestCookieCollection cookie)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/project/{projectId}/task")
        {
            Content = new StringContent(JsonConvert.SerializeObject(state), Encoding.UTF8, "application/json")
        };

        using var client = cookie.GetClientFor(apiUrl, "asc_auth_key");
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao!.Response!;
    }

    private async Task<HttpResponseMessage> UpdateTaskStatus(int id, int? status, int? statusId, IRequestCookieCollection cookie)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{apiUrl}/project/task/{id}/status")
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { status, statusId }), Encoding.UTF8, "application/json")
        };
        
        using var client = cookie.GetClientFor(apiUrl, "asc_auth_key");
        return await client.SendAsync(request);
    }
}