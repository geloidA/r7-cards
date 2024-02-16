using Newtonsoft.Json;
using Onlyoffice.Api.Models;

using Task = System.Threading.Tasks.Task;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;
using Cardmngr.Server.Enums;

namespace Cardmngr.Server.Services;

public class ProjectFileService(IConfiguration conf) : IProjectFileService
{
    public async Task<MyTask> CreateTask(string guid, UpdatedStateTask state)
    {
        var tasks = await ConvertFromFileAsync<List<MyTask>>(ConfigValues.TasksPath, "Tasks path is not configured");
        var id = await IncrementCounterAsync();

        var task = new MyTask
        {
            Title = state.Title,
            Description = state.Description,
            CustomTaskStatus = (int)Status.Todo,
            Status = 1,
            Id = id,
            CreatedBy = new UserDto { Id = guid },
            CanEdit = true,
            CanDelete = true
        };

        tasks.Add(task);

        await WriteAllTextAsync(ConfigValues.TasksPath, JsonConvert.SerializeObject(tasks));

        return task;
    }

    public async Task<MyTask> DeleteTaskAsync(int taskId)
    {
        var tasks = await ConvertFromFileAsync<List<MyTask>>(ConfigValues.TasksPath, "Tasks path is not configured");

        var task = tasks.FirstOrDefault(t => t.Id == taskId) 
            ?? throw new NullReferenceException($"Task not found by id - {taskId}");

        tasks.Remove(task);

        await WriteAllTextAsync(ConfigValues.TasksPath, JsonConvert.SerializeObject(tasks));

        return task;
    }

    public Task<Project> GetProject()
    {
        return ConvertFromFileAsync<Project>(ConfigValues.ProjectPath, "Project path is not configured");
    }

    public async IAsyncEnumerable<MyTask> GetTasksAsync(string guid)
    {
        var tasks = await ConvertFromFileAsync<List<MyTask>>(ConfigValues.TasksPath, "Tasks path is not configured");

        foreach (var task in tasks)
        {
            var canManipulate = CanManipulate(guid, task);
            task.CanEdit = canManipulate;
            task.CanDelete = canManipulate;
            yield return task;
        }
    }

    public async IAsyncEnumerable<MyTaskStatus> GetTaskStatuses()
    {
        var statuses = await ConvertFromFileAsync<List<MyTaskStatus>>(ConfigValues.StatusesPath, "Statuses path is not configured");

        foreach (var status in statuses)
            yield return status;
    }

    public async Task<MyTask> UpdateTask(int taskId, UpdatedStateTask state)
    {
        var tasks = await ConvertFromFileAsync<List<MyTask>>(ConfigValues.TasksPath, "Tasks path is not configured");

        var task = tasks.FirstOrDefault(t => t.Id == taskId) 
            ?? throw new NullReferenceException($"Task not found by id - {taskId}");

        if (task.CustomTaskStatus!.Value != (int)Status.Todo)
            throw new InvalidOperationException("Task is not in Todo status");

        task.Title = state.Title;
        task.Description = state.Description;

        await WriteAllTextAsync(ConfigValues.TasksPath, JsonConvert.SerializeObject(tasks));

        return task;
    }

    public async Task UpdateTaskStatus(int taskId, int statusId)
    {
        var tasks = await ConvertFromFileAsync<List<MyTask>>(ConfigValues.TasksPath, "Tasks path is not configured");

        var task = tasks.FirstOrDefault(t => t.Id == taskId) 
            ?? throw new NullReferenceException($"Task not found by id - {taskId}");

        task.CustomTaskStatus = statusId;

        if (statusId != (int)Status.Todo)
        {
            task.CanEdit = false;
            task.CanDelete = false;
        }
        
        await WriteAllTextAsync(ConfigValues.TasksPath, JsonConvert.SerializeObject(tasks));
    }

    private async Task WriteAllTextAsync(string pathName, string json)
    {
        var path = conf[pathName] 
            ?? throw new NullReferenceException("Path is not configured");

        await File.WriteAllTextAsync(path, json);
    }

    private bool CanManipulate(string guid, MyTask task)
    {
        if (guid == null) return false;
        var devGuid = conf[ConfigValues.DeveloperGuid];

        return devGuid == guid || task.CreatedBy?.Id == guid && task.CustomTaskStatus == (int)Status.Todo;
    }

    private async Task<int> IncrementCounterAsync()
    {
        var counterFilePath = conf[ConfigValues.CounterPath] 
            ?? throw new NullReferenceException("Counter path is not configured");

        var text = await File.ReadAllTextAsync(counterFilePath);
        var counter = int.Parse(text);
        await File.WriteAllTextAsync(counterFilePath, (counter + 1).ToString());
        return counter;
    }

    private async Task<T> ConvertFromFileAsync<T>(string path, string errorMsg)
    {
        var json = await File.ReadAllTextAsync(conf[path] 
            ?? throw new NullReferenceException(errorMsg));

        return JsonConvert.DeserializeObject<T>(json) 
            ?? throw new NullReferenceException(errorMsg);
    }
}
