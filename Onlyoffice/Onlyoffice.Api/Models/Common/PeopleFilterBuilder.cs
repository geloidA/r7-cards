namespace Onlyoffice.Api.Models.Common;

public class PeopleFilterBuilder : FilterBuilder
{
    private PeopleFilterBuilder() { }
    public static PeopleFilterBuilder Instance => new();

    public PeopleFilterBuilder GroupId(string groupId)
    {
        Filters["groupId"] = groupId;
        return this;
    }
}
