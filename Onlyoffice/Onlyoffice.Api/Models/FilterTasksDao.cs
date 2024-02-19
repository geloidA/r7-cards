namespace Onlyoffice.Api.Models;

public class FilterTasksDao
{
    public int Count { get; set; }
    public int Total { get; set; }
    public int Status { get; set; }
    public int StatusCode { get; set; }
    public List<TaskDto>? Response { get; set; }
}
