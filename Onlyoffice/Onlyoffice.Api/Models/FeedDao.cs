namespace Onlyoffice.Api.Models;

public class FeedDao : SingleResponseDao<FeedResponse> { }

public class FeedResponse
{
    public List<FeedDto>? Feeds { get; set; }
}

public class FeedDto 
{
    public FeedInfo? Feed { get; set; }
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

public class FeedInfo : IEntityDto<string>
{
    public string Item { get; set; } = "";
    public int ItemId { get; set; }
    public string Id { get; set; } = "";
    public Guid AuthorId { get; set; }
    public Guid ModifiedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string? Product { get; set; }
    public string? Module { get; set; }
    public string? ExtraLocation { get; set; }
    public string? ExtraLocationUrl { get; set; }
    public string? Title { get; set; }
    public string? ItemUrl { get; set; }
    public string? Description { get; set; }
    public string? AdditionalInfo { get; set; }
    public bool CanComment { get; set; }
    public string? CommentApiUrl { get; set; }
    public List<string>? Comments { get; set; }
}