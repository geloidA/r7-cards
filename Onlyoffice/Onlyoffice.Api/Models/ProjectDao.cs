using System.Text.Json.Serialization;

namespace Onlyoffice.Api.Models;

public class SingleProjectDao : SingleResponseDao<ProjectDto>;

public class ProjectDao : MultiResponseDao<ProjectDto>;

public class ProjectInfoDao : MultiResponseDao<ProjectInfoDto>;

public class ProjectDto : IEntityDto<int>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public UserDto? Responsible { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsFollow { get; set; }
    public DateTime Updated { get; set; }
    public UserDto? CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public int TaskCount { get; set; }
    public int TaskCountTotal { get; set; }    
}

public class ProjectInfoDto : IEntityDto<int>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public UserDto? Responsible { get; set; }
    public int? ResponsibleId { get; set; }
    public bool CanEdit { get; set; }
    public bool IsPrivate { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is ProjectInfoDto dto)
        {
            return dto.Id == Id;
        }

        return false;
    }

    public override int GetHashCode() => Id;
}

public class ProjectCreateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ResponsibleId { get; set; }
    public string? Tags { get; set; }
    [JsonPropertyName("private")] public bool IsPrivate { get; set; } = true;
}
