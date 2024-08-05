namespace Onlyoffice.Api.Models.Common;

public class FeedFilterBuilder : FilterBuilder
{
    private FeedFilterBuilder() { }
    public static FeedFilterBuilder Instance => new();

    public FeedFilterBuilder Product(string product)
    {
        Filters["product"] = product;
        return this;
    }

    public FeedFilterBuilder From(DateTime from)
    {
        Filters["from"] = from.ToString("yyyy-MM-dd");
        return this;
    }

    public FeedFilterBuilder To(DateTime to)
    {
        Filters["to"] = to.ToString("yyyy-MM-dd");
        return this;
    }

    public FeedFilterBuilder Author(string author)
    {
        Filters["author"] = author;
        return this;
    }

    public FeedFilterBuilder OnlyNew(bool onlyNew)
    {
        Filters["onlyNew"] = onlyNew.ToString();
        return this;
    }

    public FeedFilterBuilder TimeReaded(DateTime timeReaded)
    {
        Filters["timeReaded"] = timeReaded.ToString("yyyy-MM-dd");
        return this;
    }
}
