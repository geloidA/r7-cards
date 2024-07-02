namespace Onlyoffice.Api.Models.Common;

public class FeedFilterBuilder : FilterBuilder
{
    private FeedFilterBuilder() { }
    public static FeedFilterBuilder Instance => new();

    public FeedFilterBuilder Product(string product)
    {
        _filters["product"] = product;
        return this;
    }

    public FeedFilterBuilder From(DateTime from)
    {
        _filters["from"] = from.ToString("yyyy-MM-dd");
        return this;
    }

    public FeedFilterBuilder To(DateTime to)
    {
        _filters["to"] = to.ToString("yyyy-MM-dd");
        return this;
    }

    public FeedFilterBuilder Author(string author)
    {
        _filters["author"] = author;
        return this;
    }

    public FeedFilterBuilder OnlyNew(bool onlyNew)
    {
        _filters["onlyNew"] = onlyNew.ToString();
        return this;
    }

    public FeedFilterBuilder TimeReaded(DateTime timeReaded)
    {
        _filters["timeReaded"] = timeReaded.ToString("yyyy-MM-dd");
        return this;
    }
}
