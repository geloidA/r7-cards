namespace Onlyoffice.Api.Models;

public class TaskStatusDao : MultiResponseDao<TaskStatusDto>;

public class TaskStatusDto : IEntityDto<int>
{
    public int StatusType { get; set; }
    public bool CanChangeAvailable { get; set; }
    public int Id { get; set; }
    public string? Image { get; set; }
    public string? ImageType { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public int Order { get; set; }
    public bool IsDefault { get; set; }
    public bool Available { get; set; }
}