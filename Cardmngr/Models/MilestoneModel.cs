using Cardmngr.Models;
using Cardmngr.Models.Attributes;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr;

public class MilestoneModel : ModelBase
{
    public int Id { get; }
    [Updatable]
    public string? Title { get; set; }
    [Updatable]
    public string? Description { get; set; }
    public ProjectModel Project { get; }
    [Updatable]
    public DateTime? Deadline { get; set; }
    [Updatable]
    public bool IsKey { get; set; }
    [Updatable]
    public bool IsNotify { get; set; }
    
    public int ActiveTaskCount => Tasks.Where(x => x.StatusColumn.StatusType == Status.Open).Count();
    public int ClosedTaskCount => Tasks.Where(x => x.StatusColumn.StatusType == Status.Closed).Count();

    [Updatable]
    public Status Status { get; set; }
    [Updatable]
    public IUser? Responsible { get; set; }

    public bool IsSelected { get; private set; }

    public IEnumerable<TaskModel> Tasks => Project.Tasks.Where(x => x.Milestone == this);

    public void Update(Milestone milestone)
    {
        ArgumentNullException.ThrowIfNull(milestone);

        UpdateProperties(milestone);

        Project.OnModelChanged();
    }

    public DateTime Start 
    {
        get
        {
            return Project.Tasks
                .Where(x => x.Milestone == this && x.StartDate.HasValue)
                .Select(x => x.StartDate!.Value)
                .Concat([Deadline!.Value.AddDays(-7)])
                .Min();
        }
    }

    public MilestoneModel(Milestone milestone, ProjectModel project)
    {
        Id = milestone.Id;
        Title = milestone.Title;
        Description = milestone.Description;
        Project = project;
        Deadline = milestone.Deadline;
        IsKey = milestone.IsKey;
        IsNotify = milestone.IsNotify;
        Status = (Status)milestone.Status;
        Responsible = milestone.Responsible != null ? new User(milestone.Responsible) : null;
        Created = milestone.Created;
        CreatedBy = milestone.CreatedBy != null ? new User(milestone.CreatedBy) : null;
        Updated = milestone.Updated;
    }

    private MilestoneModel(MilestoneModel source)
    {
        Id = source.Id;
        Title = source.Title;
        Description = source.Description;
        Project = source.Project;
        Deadline = source.Deadline;
        IsKey = source.IsKey;
        IsNotify = source.IsNotify;
        Status = source.Status;
        Responsible = source.Responsible != null ? new User(source.Responsible) : null;
        Created = source.Created;
        CreatedBy = source.CreatedBy != null ? new User(source.CreatedBy) : null;;
        Updated = source.Updated;
    }

    public MilestoneModel Clone() => new(this);

    public void ToggleSelection() => IsSelected = !IsSelected;
}
