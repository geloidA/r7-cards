﻿using System.Text;
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

    [HttpPost]
    [Route("api/[controller]/{projectId}/task")]
    public Task ProxyCreateTask(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/task");

    [HttpDelete]
    [Route("api/[controller]/task/{taskId}")]
    public Task ProxyDeleteTask(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}");

    [HttpPut]
    [Route("api/[controller]/task/{taskId}")]
    public Task ProxyUpdateTask(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}");
    #endregion

    #region Subtask Proxy
    [HttpPut]
    [Route("api/[controller]/task/{taskId}/{subtaskId}")]
    public Task ProxyUpdateSubtask(int taskId, int subtaskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/{subtaskId}");

    [HttpPut]
    [Route("api/[controller]/task/{taskId}/{subtaskId}/status")]
    public Task ProxyUpdateSubtaskStatus(int taskId, int subtaskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/{subtaskId}/status");

    [HttpDelete]
    [Route("api/[controller]/task/{taskId}/{subtaskId}")]
    public Task ProxyDeleteSubtask(int taskId, int subtaskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}/{subtaskId}");

    [HttpPost]
    [Route("api/[controller]/task/{taskId}")]
    public Task ProxyCreateSubtask(int taskId) => ProxyRequestAsync($"{apiUrl}/project/task/{taskId}");
    #endregion

    #region Milestone Proxy
    [HttpGet]
    [Route("api/[controller]/{projectId}/milestone")]
    public Task ProxyMilestones(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/milestone");

    [HttpPost]
    [Route("api/[controller]/{projectId}/milestone")]
    public Task ProxyCreateMilestone(int projectId) => ProxyRequestAsync($"{apiUrl}/project/{projectId}/milestone");

    [HttpDelete]
    [Route("api/[controller]/milestone/{milestoneId}")]
    public Task ProxyDeleteMilestone(int milestoneId) => ProxyRequestAsync($"{apiUrl}/project/milestone/{milestoneId}");

    [HttpPut]
    [Route("api/[controller]/milestone/{milestoneId}/status")]
    public Task ProxyUpdateMilestoneStatus(int milestoneId) => ProxyRequestAsync($"{apiUrl}/project/milestone/{milestoneId}/status");

    [HttpPut]
    [Route("api/[controller]/milestone/{milestoneId}")]
    public Task ProxyUpdateMilestone(int milestoneId) => ProxyRequestAsync($"{apiUrl}/project/milestone/{milestoneId}");
    #endregion

    [HttpPost]
    [Route("api/[controller]/{projectId}/task/status")]
    public async Task CreateTaskWithStatus(int projectId)
    {
        var body = await ConvertStreamToDynamicAsync(HttpContext.Request.Body);
        var cookieCollection = HttpContext.Request.Cookies;
        var task = await CreateTaskAsync(projectId, body.state.ToObject<TaskUpdateData>(), cookieCollection); // TODO: refactor
        var response = await UpdateTaskStatus(task.Id, (int)body.status, (int?)body.statusId, cookieCollection);
        var str = await response.Content.ReadAsStringAsync();

        await HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(str));
    }

    private static async Task<dynamic> ConvertStreamToDynamicAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var requestBody = await reader.ReadToEndAsync();
        return JsonConvert.DeserializeObject(requestBody) ?? throw new NullReferenceException("Request body is null");
    }

    private async Task<Api.Models.TaskDto> CreateTaskAsync(int projectId, TaskUpdateData state, IRequestCookieCollection cookie)
    {
        using var client = cookie.GetClientFor(apiUrl);
        var httpContent = new StringContent(JsonConvert.SerializeObject(state), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/project/{projectId}/task")
        {
            Content = httpContent
        };
        var response = await client.SendAsync(request);
        var taskDao = await response.Content.ReadFromJsonAsync<SingleTaskDao>();
        return taskDao?.Response ?? throw new NullReferenceException("Task was not created");
    }

    private async Task<HttpResponseMessage> UpdateTaskStatus(int id, int status, int? statusId, IRequestCookieCollection cookie)
    {
        using var client = cookie.GetClientFor(apiUrl);
        var httpContent = new StringContent(JsonConvert.SerializeObject(new { status, statusId }), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, $"{apiUrl}/project/task/{id}/status")
        {
            Content = httpContent
        };
        var response = await client.SendAsync(request);
        return response;
    }
}