namespace Onlyoffice.Api.Models;

public class SingleProjectDao : HttpResponseDaoBase
{
    public Project? Response { get; set; }
}

public class ProjectDao : HttpResponseDaoBase
{
    public List<Project>? Response { get; set; }
}

public class Project
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public Responsible? Responsible { get; set; }
    public bool CanEdit { get; set; }
    public bool IsPrivate { get; set; }
    public DateTime Updated { get; set; }
    public CreatedBy? CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public int TaskCount { get; set; }
    public int TaskCountTotal { get; set; }    
}

public class Responsible : IUser
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarSmall { get; set; }
    public string? ProfileUrl { get; set; }
}
