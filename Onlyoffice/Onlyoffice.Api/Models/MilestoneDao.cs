namespace Onlyoffice.Api.Models;

public class SingleMilestoneDao : SingleResponseDao<MilestoneDto> { }

public class MilestoneDao : MultiResponseDao<MilestoneDto> { }

public class MilestoneDto : IEntityDto<int>
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ProjectInfoDto? ProjectOwner { get; set; }
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
}

public class MilestoneUpdateData
{
    public string? Description { get; set; }
    public string? Title { get; set; }
    public bool IsKey { get; set; }
    public int Status { get; set; }
    public bool IsNotify { get; set; }
    public DateTime? Deadline { get; set; }
    public int ProjectId { get; set; }
    public string? Responsible { get; set; }
    public bool NotifyResponsible { get; set; }
}
