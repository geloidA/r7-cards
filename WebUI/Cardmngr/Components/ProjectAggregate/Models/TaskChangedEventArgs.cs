using Cardmngr.Domain.Entities;

namespace Cardmngr.Components.ProjectAggregate.Models;

public class TaskChangedEventArgs : EventArgs
{
    public TaskChangedEventArgs()
    {
        
    }

    public TaskChangedEventArgs(TaskAction action, OnlyofficeTask task)
    {
        Action = action;
        Task = task;
    }

    public TaskAction Action { get; set; }

    public OnlyofficeTask? Task { get; set; }
}

public enum TaskAction
{
    None = 0,
    Add = 1,
    Update = 2,
    ChangeStatus = 4,
    Remove = 8
}