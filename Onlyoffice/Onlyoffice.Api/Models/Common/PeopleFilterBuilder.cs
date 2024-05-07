namespace Onlyoffice.Api;

public class PeopleFilterBuilder : FilterBuilder
{
    private PeopleFilterBuilder() { }
    public static PeopleFilterBuilder Instance => new();

    public PeopleFilterBuilder GroupId(string groupId)
    {
        _filters["groupId"] = groupId;
        return this;
    }
}
