using System.Collections;
using Cardmngr.Extensions;
using Onlyoffice.Api.Common;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Models;

public class TaskStatusColumn : ModelBase, IEnumerable<TaskModel>
{
    private readonly List<TaskModel> tasks;

    public TaskStatusColumn(MyTaskStatus taskStatus, IEnumerable<Onlyoffice.Api.Models.Task> tasks, ProjectModel projectModel)
    {
        Project = projectModel;
        
        this.tasks = tasks
            .Select(x => new TaskModel(x, this))
            .ToList();
        
        StatusType = (Status)taskStatus.StatusType;
        CanChangeAvailable = taskStatus.CanChangeAvailable;
        Id = taskStatus.Id;
        Image = taskStatus.Image;
        ImageType = taskStatus.ImageType;
        Title = taskStatus.Title;
        Description = taskStatus.Description;
        Color = taskStatus.Color;
        Order = taskStatus.Order;
        IsDefault = taskStatus.IsDefault;
        Available = taskStatus.Available;
    }

    public Status StatusType { get; }
    public bool CanChangeAvailable { get; }
    public int Id { get; }
    public string? Image { get; }
    public string? ImageType { get; }
    public string? Title { get; }
    public string? Description { get; }
    public string? Color { get; }
    public int Order { get; }
    public bool IsDefault { get; }
    public bool Available { get; }
    public ProjectModel Project { get; }

    public void Add(TaskModel task)
    {
        if (tasks.Contains(task))
            throw new InvalidOperationException("Task already exists");

        tasks.Add(task);
        Project.OnModelChanged();
    }

    public bool Remove(TaskModel task) 
    {
        if (tasks.Remove(task))
        {
            Project.OnModelChanged();
            return true;
        }

        return false;
    }

    public int Count => tasks.Count;

    public IEnumerator<TaskModel> GetEnumerator() 
    {
        foreach (var task in tasks.OrderByProperties())
        {
            yield return task;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override bool Equals(object? obj)
    {
        if (obj is not TaskStatusColumn other)
            return false;
        return other.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}