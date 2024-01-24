using Cardmngr.Models;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr;

public class MilestoneModel(Milestone milestone) : ModelBase
{
    public int Id { get; } = milestone.Id;
    public string? Title { get; set; } = milestone.Title;
    public string? Description { get; set; } = milestone.Description;
    public ProjectOwner ProjectOwner { get; } = milestone.ProjectOwner!; // TODO: fix
    public DateTime? Deadline { get; set; } = milestone.Deadline;
    public bool IsKey { get; set; } = milestone.IsKey;
    public bool IsNotify { get; set; } = milestone.IsNotify;
    public int ActiveTaskCount { get; set; } = milestone.ActiveTaskCount;
    public int ClosedTaskCount { get; set; } = milestone.ClosedTaskCount;
    public CommonStatus Status { get; set; } = (CommonStatus)milestone.Status;
    public User Responsible { get; set; } = new User(milestone.Responsible!);
    public DateTime Created { get; } = milestone.Created;
    public User CreatedBy { get; } = new User(milestone.CreatedBy!);
    public DateTime Updated { get; set; } = milestone.Updated;

    public bool IsSelected { get; private set; }

    public void ToggleSelection() => IsSelected = !IsSelected;
}
