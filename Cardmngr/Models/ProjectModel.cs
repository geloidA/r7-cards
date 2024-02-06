using System.Collections;
using Cardmngr.Models;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr;

public class ProjectModel : ModelBase, IWorkContainer
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
        CanDelete = project.CanDelete;
    }

    public static ProjectModel Empty => new();

    private ProjectModel()
    {
        statusColumns = new StatusColumnsModel(Enumerable.Empty<MyTask>().ToList(), Enumerable.Empty<MyTaskStatus>().ToList(), this);
    }

    public int Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; } 
    public ProjectStatus Status { get; set; }
    public IUser Responsible { get; set; } 
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

    public DateTime? StartDate => Tasks.Select(x => x.StartDate).Min();

    public DateTime? Deadline => Tasks.Select(x => x.Deadline).Max();

    public bool DeleteMilestone(MilestoneModel milestone)
    {
        foreach (var task in Tasks.Where(x => x.Milestone == milestone))
        {
            task.Milestone = null;
        }

        if (milestoneTimeline.DeleteMilestone(milestone))
        {
            OnModelChanged();
            return true;
        }

        return false;
    }

    public void AddMilestone(MilestoneModel milestone)
    {
        if (milestone.Project != this)
            throw new ArgumentException("Milestone already belongs to another project");
        milestoneTimeline.AddMilestone(milestone);
    }

    public event Action? ModelChanged;

    internal void OnModelChanged() => ModelChanged?.Invoke();

    public IEnumerator<IWork> GetEnumerator()
    {
        foreach (var task in Tasks)
            yield return task;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool IsClosed() => Status == ProjectStatus.Closed;
}
