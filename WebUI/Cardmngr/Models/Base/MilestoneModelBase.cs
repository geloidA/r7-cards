using System.Collections;
using System.ComponentModel.DataAnnotations;
using Cardmngr.Models.Attributes;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models.Base;

public abstract class MilestoneModelBase<TApiModel>(IProjectModel project) : EditableModelBase<TApiModel>, IMilestoneModel
{
    public MilestoneModelBase(TApiModel milestone, IProjectModel project) : this(project)
    {
        Status = GetStatusFromApiModel(milestone);
        Update(milestone);
    }

    public IProjectModel Project { get; } = project;

    [Updatable]
    public bool IsKey { get; set; }

    [Updatable]
    public bool IsNotify { get; set; }

    public Status Status { get; set; }

    [Updatable]
    [Required(ErrorMessage = "Ответственный обязателен для заполнения")]
    public IUser? Responsible { get; set; }

    public IEnumerable<ITaskModel> Tasks => Project
        .Cast<ITaskModel>()
        .Where(x => x.Milestone?.Id == Id);

    public DateTime? StartDate
    {
        get
        {
            return Tasks
                .Where(x => x.StartDate.HasValue)
                .Select(x => x.StartDate)
                .Concat([Deadline is { } ? Deadline.Value.AddDays(-7) : null])
                .Min();
        }
    }

    [Updatable]
    [Required(ErrorMessage = "Крайний срок обязательный для заполнения")]
    public DateTime? Deadline { get; set; }

    public IEnumerator<IWork> GetEnumerator()
    {   
        foreach (var task in Tasks)
            yield return task;
    }

    public bool IsClosed() => Status == Status.Closed;

    protected abstract Status GetStatusFromApiModel(TApiModel apiModel);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
