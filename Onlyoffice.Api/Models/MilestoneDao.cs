namespace Onlyoffice.Api.Models;

public class SingleMilestoneDao : HttpResponseDaoBase
{
    public Milestone? Response { get; set; }
}

public class MilestoneDao : HttpResponseDaoBase
{
    public List<Milestone>? Response { get; set; }
}

public class Milestone
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ProjectOwner? ProjectOwner { get; set; }
    public DateTime? Deadline { get; set; }
    public bool IsKey { get; set; }
    public bool IsNotify { get; set; }
    public int ActiveTaskCount { get; set; }
    public int ClosedTaskCount { get; set; }
    public int Status { get; set; }
    public Responsible? Responsible { get; set; }
    public DateTime Created { get; set; }
    public CreatedBy? CreatedBy { get; set; }
    public DateTime Updated { get; set; }

    public Milestone FullCopy()
    {
        return new Milestone
        {
            CanEdit = CanEdit,
            CanDelete = CanDelete,
            Id = Id,
            Title = Title,
            Description = Description,
            ProjectOwner = ProjectOwner?.FullCopy(),
            Deadline = Deadline,
            IsKey = IsKey,
            IsNotify = IsNotify,
            ActiveTaskCount = ActiveTaskCount,
            ClosedTaskCount = ClosedTaskCount,
            Status = Status,
            Responsible = Responsible?.FullCopy(),
            Created = Created,
            CreatedBy = CreatedBy?.FullCopy(),
            Updated = Updated
        };
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Milestone other) return false;
        return other.Id == Id;
    }

    public UpdatedStateMilestone GetUpdatedState()
    {
        return new UpdatedStateMilestone
        {
            Description = Description,
            Title = Title,
            IsKey = IsKey,
            Status = Status,
            IsNotify = IsNotify,
            Deadline = Deadline,
            ProjectID = ProjectOwner?.Id ?? throw new Exception("ProjectOwner is null when call GetUpdatedState"),
            Responsible = Responsible?.Id,
            NotifyResponsible = IsNotify
        };
    }
}

public class UpdatedStateMilestone
{
    public string? Description { get; set; }
    public string? Title { get; set; }
    public bool IsKey { get; set; }
    public int Status { get; set; }
    public bool IsNotify { get; set; }
    public DateTime? Deadline { get; set; }
    public int ProjectID { get; set; }
    public string? Responsible { get; set; }
    public bool NotifyResponsible { get; set; }
}

public class ProjectOwner
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public bool IsPrivate { get; set; }

    public ProjectOwner FullCopy()
    {
        return new ProjectOwner
        {
            Id = Id,
            Title = Title,
            Status = Status,
            IsPrivate = IsPrivate
        };
    }
}
