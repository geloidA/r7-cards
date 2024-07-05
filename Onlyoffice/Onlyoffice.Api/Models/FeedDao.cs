namespace Onlyoffice.Api.Models;

public class FeedDao : SingleResponseDao<FeedResponse> { }

public class FeedResponse
{
    public List<FeedDto>? Feeds { get; set; }
}

public class FeedDto 
{
    public string? Feed { get; set; }
    public string? Module { get; set; }
    public bool IsToday { get; set; }
    public bool IsYesterday { get; set; }
    public bool IsTomorrow { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime AggregatedDate { get; set; }
    public DateTime? TimeReaded { get; set; }
    public string? GroupId { get; set; }
    public List<FeedDto>? GroupedFeeds { get; set; }
}