namespace Onlyoffice.Api.Models;

public class SingleProjectDao : HttpResponseDaoBase
{
    public ProjectDto? Response { get; set; }
}

public class ProjectDao : HttpResponseDaoBase
{
    public List<ProjectDto>? Response { get; set; }
}

public class ProjectInfoDao : HttpResponseDaoBase
{
    public List<ProjectInfoDto>? Response { get; set; }
}

public class ProjectDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public UserDto? Responsible { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool IsPrivate { get; set; }
    public DateTime Updated { get; set; }
    public UserDto? CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public int TaskCount { get; set; }
    public int TaskCountTotal { get; set; }    
}

public class ProjectInfoDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public UserDto? Responsible { get; set; }
    public int? ResponsibleId { get; set; }
    public bool CanEdit { get; set; }
    public bool IsPrivate { get; set; }
}
