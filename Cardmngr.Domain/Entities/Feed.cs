namespace Cardmngr.Domain.Entities;

public record Feed
{
    public FeedInfo Value { get; init; }
    public string Module { get; init; }
    public bool IsToday { get; init; }
    public bool IsYesterday { get; init; }
    public bool IsTomorrow { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime ModifiedDate { get; init; }
    public DateTime AggregatedDate { get; init; }
    public DateTime? TimeReaded { get; init; }
    public string GroupId { get; init; }
    public List<Feed> GroupedFeeds { get; init; }
}

public record FeedInfo
{
    public string Item { get; init; } = "";
    public int ItemId { get; init; }
    public string Id { get; init; } = "";
    public Guid AuthorId { get; init; }
    public Guid ModifiedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime ModifiedDate { get; init; }
    public string Product { get; init; }
    public string Module { get; init; }
    public string ExtraLocation { get; init; }
    public string ExtraLocationUrl { get; init; }
    public string Title { get; init; }
    public string ItemUrl { get; init; }
    public string Description { get; init; }
    public string AdditionalInfo { get; init; }
    public bool CanComment { get; init; }
    public string CommentApiUrl { get; init; }
    public List<string> Comments { get; init; }
}
