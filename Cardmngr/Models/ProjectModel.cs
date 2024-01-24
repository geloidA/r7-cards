using Cardmngr.Models;
using Cardmngr.Utils;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr;

public class ProjectModel : ModelBase
{
    private readonly List<TaskStatusColumn> statusColumns;
    private readonly HashSet<MilestoneModel> milestones;

    public ProjectModel(Project project, List<MyTask> tasks, List<MyTaskStatus> statuses, List<Milestone> milestones)
    {
        this.milestones = milestones
            .Select(m => new MilestoneModel(m))
            .ToHashSet();
        
        statusColumns = statuses
            .OrderBy(x => (x.StatusType, x.Order))
            .Select(x => new TaskStatusColumn(x, tasks.FilterByStatus(x), this))
            .ToList();

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
    public User Responsible { get; set; } 
    public bool CanEdit { get; } 
    public bool IsPrivate { get; set; } 
    public DateTime Updated { get; }
    public User CreatedBy { get; }
    public DateTime Created { get; }
    public int TaskCount { get; private set; }
    public int TaskCountTotal { get; private set; }

    public IEnumerable<MilestoneModel> Milestones
    {
        get
        {
            foreach (var milestone in milestones)
                yield return milestone;
        }
    }
}
