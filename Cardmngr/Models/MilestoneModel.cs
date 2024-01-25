using Cardmngr.Models;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr;

public class MilestoneModel : ModelBase
{
    public int Id { get; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ProjectModel Project { get; }
    public DateTime? Deadline { get; set; }
    public bool IsKey { get; set; }
    public bool IsNotify { get; set; }
    public int ActiveTaskCount => Project.Tasks.Where(x => x.StatusColumn.StatusType == Status.Open).Count();
    public int ClosedTaskCount => Project.Tasks.Where(x => x.StatusColumn.StatusType == Status.Closed).Count();
    public Status Status { get; set; }
    public IUser Responsible { get; set; }

    public bool IsSelected { get; private set; }

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
        Responsible = new User(milestone.Responsible!);
        Created = milestone.Created;
        CreatedBy = new User(milestone.CreatedBy!);
        Updated = milestone.Updated;
    }

    public void ToggleSelection() => IsSelected = !IsSelected;
}
