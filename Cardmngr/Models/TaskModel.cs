﻿using System.Collections.ObjectModel;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

public class TaskModel : ModelBase
{
    public TaskModel(Onlyoffice.Api.Models.Task task, TaskStatusColumn statusColumn)
    {
        this.statusColumn = statusColumn;
        CanEdit = task.CanEdit;
        CanCreateSubtask = task.CanCreateSubtask;
        CanCreateTimeSpend = task.CanCreateTimeSpend;
        CanDelete = task.CanDelete;
        CanReadFiles = task.CanReadFiles;
        Id = task.Id;
        Title = task.Title;
        Description = task.Description;
        Priority = (TaskPriority)task.Priority;
        Milestone = statusColumn.Project.Milestones.FirstOrDefault(m => m.Id == task.MilestoneId);
        ProjectOwner = statusColumn.Project;
        Subtasks = new ObservableCollection<SubtaskModel>(task.Subtasks?.Select(s => new SubtaskModel(s)) ?? []);
        Progress = task.Progress;
        UpdatedBy = task.UpdatedBy == null ? null : new User(task.UpdatedBy);
        Created = task.Created;
        CreatedBy = new User(task.CreatedBy!);
        Updated = task.Updated;
        Responsibles = new ObservableCollection<IUser>(task.Responsibles?.Select(u => new User(u)) ?? []);
        Deadline = task.Deadline;
        StartDate = task.StartDate;
    }

    private TaskModel(TaskModel source, bool copySubtasks)
    {
        statusColumn = source.statusColumn;
        CanEdit = source.CanEdit;
        CanCreateSubtask = source.CanCreateSubtask;
        CanCreateTimeSpend = source.CanCreateTimeSpend;
        CanDelete = source.CanDelete;
        CanReadFiles = source.CanReadFiles;
        Id = source.Id;
        Title = source.Title;
        Description = source.Description;
        Priority = source.Priority;
        Milestone = source.Milestone; // Не используем новый объект, а ссылку на существующий
        ProjectOwner = source.ProjectOwner; // То же самое
        Subtasks = copySubtasks ? new ObservableCollection<SubtaskModel>(source.Subtasks?.Select(s => s.Clone()) ?? []) : source.Subtasks;
        Progress = source.Progress;
        UpdatedBy = source.UpdatedBy;
        Created = source.Created;
        CreatedBy = source.CreatedBy;
        Updated = source.Updated;
        Responsibles = new ObservableCollection<IUser>(source.Responsibles?.Select(u => new User(u)) ?? []);
        Deadline = source.Deadline;
        StartDate = source.StartDate;
    }

    #region Properties
    public bool CanEdit { get; }
    public bool CanCreateSubtask { get; }
    public bool CanCreateTimeSpend { get; }
    public bool CanDelete { get; }
    public bool CanReadFiles { get; }
    public int Id { get; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public MilestoneModel? Milestone { get; set; }
    public ProjectModel ProjectOwner { get; }
    public ObservableCollection<SubtaskModel> Subtasks { get; set; }
    public int? Progress { get; set; }
    public IUser? UpdatedBy { get; private set; }
    public ObservableCollection<IUser> Responsibles { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? StartDate { get; set; }
    #endregion

    /// <summary>
    /// Updates the task model with only changeble properties
    /// </summary>
    /// <remarks>
    /// Use this method if you need change several properties with one invoke OnModelChanged method
    /// </remarks>
    /// <param name="task"></param>
    public void Update(Onlyoffice.Api.Models.Task task)
    {
        ArgumentNullException.ThrowIfNull(task);

        Title = task.Title;
        Description = task.Description;
        Priority = (TaskPriority)task.Priority;
        Milestone = ProjectOwner.Milestones.FirstOrDefault(m => m.Id == task.MilestoneId);
        Progress = task.Progress;
        Updated = task.Updated;
        UpdatedBy = task.UpdatedBy == null ? null : new User(task.UpdatedBy);
        Subtasks = new ObservableCollection<SubtaskModel>(task.Subtasks?.Select(s => new SubtaskModel(s)) ?? []);
        Responsibles = new ObservableCollection<IUser>(task.Responsibles?.Select(u => new User(u)) ?? []);
        Deadline = task.Deadline;
        StartDate = task.StartDate;

        OnModelChanged();
    }

    /// <summary>
    /// <b>Warning</b>: not copy ProjectOwner, Milestone
    /// </summary>
    /// <returns></returns>
    public TaskModel Clone(bool copySubtasks) => new(this, copySubtasks);
    
    private TaskStatusColumn statusColumn;
    public TaskStatusColumn StatusColumn 
    {
        get => statusColumn;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            statusColumn.Remove(this);
            value.Add(this);

            statusColumn = value;
        }
    }

    public bool IsDeadlineOut
    {
        get
        {
            return Deadline.HasValue && 
                statusColumn.StatusType != Status.Closed && 
                DateTime.Now.Date > Deadline;
        }
    }

    public bool CanMarkClosed
    {
        get
        {
            return statusColumn.StatusType == Status.Closed
                || (Subtasks?.All(x => x.Status == Status.Closed) ?? false);
        }
    }

    public void CloseAllSubtasks()
    {
        if (Subtasks is not { } || Subtasks.Count == 0)
            throw new InvalidOperationException("No subtasks found");

        foreach (var subtask in Subtasks)
        {
            subtask.Status = Status.Closed;
        }
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TaskModel other) return false;
        return other.Id == Id;
    }
}