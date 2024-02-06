using System.Collections;
using System.ComponentModel.DataAnnotations;
using Cardmngr.Models;
using Cardmngr.Models.Attributes;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr;

public class MilestoneModel : ModelBase, IWorkContainer
{
    [Updatable]
    public int Id { get; private set; }
    [Updatable]
    [Required(ErrorMessage = "Название обязательное для заполнения")]
    public string Title { get; set; }
    [Updatable]
    public string? Description { get; set; }
    public ProjectModel Project { get; }
    [Updatable]
    [Required(ErrorMessage = "Крайний срок обязательный для заполнения")]
    public DateTime? Deadline { get; set; }
    [Updatable]
    public bool IsKey { get; set; }
    [Updatable]
    public bool IsNotify { get; set; }

    public Status Status { get; set; }
    [Updatable]
    [Required(ErrorMessage = "Ответственный обязателен для заполнения")]
    public IUser? Responsible { get; set; }

    public bool IsSelected { get; private set; }

    public IEnumerable<TaskModel> Tasks => Project.Tasks.Where(x => x.Milestone?.Id == Id);

    public void Update(Milestone milestone)
    {
        ArgumentNullException.ThrowIfNull(milestone);

        UpdateProperties(milestone);

        GetMilestoneStatus(milestone.Status);

        Project.OnModelChanged();
    }

    public DateTime? StartDate // TODO: O(n) complexity
    {
        get
        {
            return Project.Tasks
                .Where(x => x.Milestone?.Id == Id && x.StartDate.HasValue)
                .Select(x => x.StartDate)
                .Concat([Deadline is { } ? Deadline.Value.AddDays(-7) : null])
                .Min();
        }
    }

    public MilestoneModel(Milestone milestone, ProjectModel project)
    {
        Id = milestone.Id;
        Title = milestone.Title ?? milestone.Id.ToString();
        Description = milestone.Description;
        Project = project;
        Deadline = milestone.Deadline ?? throw new ArgumentNullException("Milestone deadline cannot be null");
        IsKey = milestone.IsKey;
        IsNotify = milestone.IsNotify;
        Status = GetMilestoneStatus(milestone.Status);
        Responsible = milestone.Responsible != null ? new User(milestone.Responsible) : null;
        Created = milestone.Created;
        CreatedBy = milestone.CreatedBy != null ? new User(milestone.CreatedBy) : null;
        Updated = milestone.Updated;
        CanEdit = milestone.CanEdit;
        CanDelete = milestone.CanDelete;
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
        CanEdit = source.CanEdit;
        CanDelete = source.CanDelete;
    }

    public static MilestoneModel Create(ProjectModel project, bool canManipulate = false) => new(project, canManipulate);

    private MilestoneModel(ProjectModel project, bool canManipulate)
    {
        Project = project;
        Title = string.Empty;
        CanEdit = canManipulate;
        CanDelete = canManipulate;
    }

    public MilestoneModel Clone() => new(this);

    public void ToggleSelection() => IsSelected = !IsSelected;

    public IEnumerator<IWork> GetEnumerator()
    {
        foreach (var task in Tasks)
            yield return task;
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool IsClosed() => Status == Status.Closed;

    private static Status GetMilestoneStatus(int status) => status == 0 ? Status.Open : Status.Closed;
}
