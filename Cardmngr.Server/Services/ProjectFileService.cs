using System.Text.Json;
using Onlyoffice.Api.Models;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Server.Services;

public class ProjectFileService(IConfiguration conf) : IProjectFileService
{
    public Task<Project> GetProject()
    {
        return ConvertFromFileAsync<Project>("project-path", "Project path is not configured");
    }

    public async IAsyncEnumerable<MyTask> GetTasksAsync()
    {
        var tasks = await ConvertFromFileAsync<List<MyTask>>("tasks-path", "Tasks path is not configured");

        foreach (var task in tasks)
            yield return task;
    }

    public async IAsyncEnumerable<MyTaskStatus> GetTaskStatuses()
    {
        var statuses = await ConvertFromFileAsync<List<MyTaskStatus>>("statuses-path", "Statuses path is not configured");

        foreach (var status in statuses)
            yield return status;
    }

    private async Task<T> ConvertFromFileAsync<T>(string path, string errorMsg)
    {
        var json = await File.ReadAllTextAsync(conf[path] ?? throw new NullReferenceException(errorMsg));

        return JsonSerializer.Deserialize<T>(json) ?? throw new InvalidOperationException($"Can't convert {path} json to object");
    }
}
