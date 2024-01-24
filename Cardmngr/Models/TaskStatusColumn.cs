using Onlyoffice.Api.Common;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Models;

public class TaskStatusColumn(MyTaskStatus taskStatus, IEnumerable<Onlyoffice.Api.Models.Task> tasks, ProjectModel projectModel) : ModelBase
{
    private readonly List<TaskModel> tasks = tasks
        .Select(x => new TaskModel(x, projectModel))
        .ToList();

    public CommonStatus StatusType { get; } = (CommonStatus)taskStatus.StatusType;
    public bool CanChangeAvailable { get; } = taskStatus.CanChangeAvailable;
    public int Id { get; } = taskStatus.Id;
    public string? Image { get; } = taskStatus.Image;
    public string? ImageType { get; } = taskStatus.ImageType;
    public string? Title { get; } = taskStatus.Title;
    public string? Description { get; } = taskStatus.Description;
    public string? Color { get; } = taskStatus.Color;
    public int Order { get; } = taskStatus.Order;
    public bool IsDefault { get; } = taskStatus.IsDefault;
    public bool Available { get; } = taskStatus.Available;

    public IEnumerable<TaskModel> Tasks
    {
        get
        {
            foreach (var task in tasks)
                yield return task;
        }
    }
}
