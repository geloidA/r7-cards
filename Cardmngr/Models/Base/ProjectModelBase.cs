using System.Collections;
using Cardmngr.Models.Attributes;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models.Base;

public abstract class ProjectModelBase : ModelBase, IProjectModel
{
    public ProjectModelBase(Project project)
    {
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

    [Updatable]
    public ProjectStatus Status { get; set; }

    [Updatable]
    public IUser Responsible { get; set; }

    [Updatable]
    public bool IsPrivate { get; set; }

    public DateTime? StartDate => this.Min(x => x.StartDate);

    public DateTime? Deadline => this.Max(x => x.Deadline);

    public bool IsClosed() => Status == ProjectStatus.Closed;

    public IEnumerator<IWork> GetEnumerator()
    {
        foreach (var task in StatusBoard.SelectMany(x => x))
        {
            yield return task;
        }
    }

    public abstract IEnumerable<IUser> Team { get; }
    public abstract IStatusColumnBoard StatusBoard { get; }
    public abstract IObservableCollection<IMilestoneModel> Milestones { get; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
