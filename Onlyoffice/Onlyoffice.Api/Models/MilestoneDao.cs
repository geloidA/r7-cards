namespace Onlyoffice.Api.Models;

public class SingleMilestoneDao : HttpResponseDaoBase
{
    public MilestoneDto? Response { get; set; }
}

public class MilestoneDao : HttpResponseDaoBase
{
    public List<MilestoneDto>? Response { get; set; }
}

public class MilestoneDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Project? Project { get; set; }
    public DateTime? Deadline { get; set; }
    public bool IsKey { get; set; }
    public bool IsNotify { get; set; }
    public int ActiveTaskCount { get; set; }
    public int ClosedTaskCount { get; set; }
    public int Status { get; set; }
    public UserDto? Responsible { get; set; }
    public DateTime Created { get; set; }
    public UserDto? CreatedBy { get; set; }
    public DateTime Updated { get; set; }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not MilestoneDto other) return false;
        return other.Id == Id;
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
