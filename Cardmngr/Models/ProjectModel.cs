﻿using Cardmngr.Models;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr;

public class ProjectModel : ModelBase
{
    private readonly MilestoneTimelineModel milestoneTimeline;
    private readonly StatusColumnsModel statusColumns;
    private readonly List<IUser> team;

    public ProjectModel(Project project, List<MyTask> tasks, List<MyTaskStatus> statuses, List<Milestone> milestones, IEnumerable<IUser> team)
    {
        milestoneTimeline = new MilestoneTimelineModel(milestones, this);
        this.team = team.ToList();
        
        statusColumns = new StatusColumnsModel(tasks, statuses, this);

        Id = project.Id;
        Title = project.Title ?? string.Empty;
        Description = project.Description;
        Status = (ProjectStatus)project.Status;
        Responsible = new User(project.Responsible!);
        CanEdit = project.CanEdit;
        IsPrivate = project.IsPrivate;
        Updated = project.Updated;
        CreatedBy = new User(project.CreatedBy!);
        Created = project.Created;
    }

    public int Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; } 
    public ProjectStatus Status { get; set; } 
    public IUser Responsible { get; set; } 
    public bool CanEdit { get; } 
    public bool IsPrivate { get; set; }
    
    public IEnumerable<IUser> Team
    {
        get
        {
            foreach (var user in team)
                yield return user;
        }
    }

    public int TeamCount => team.Count;

    public int TaskCount => statusColumns
        .Where(x => x.StatusType == Onlyoffice.Api.Common.Status.Open)
        .Sum(x => x.Count);

    public int TaskCountTotal => statusColumns.Sum(x => x.Count);

    public IEnumerable<TaskModel> Tasks => statusColumns.SelectMany(x => x);

    public MilestoneTimelineModel Milestones => milestoneTimeline;
    public StatusColumnsModel StatusColumns => statusColumns;
}