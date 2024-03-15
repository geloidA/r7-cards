using System.Text;

namespace Onlyoffice.Api.Common;

public class FilterTasksBuilder
{
    private readonly StringBuilder urlBuilder = new("filter/");
    
    private FilterTasksBuilder() { }
    public static FilterTasksBuilder Instance => new();

    public FilterTasksBuilder WithProjectId(int? projectId)
    {
        if (projectId is null) return this;
        urlBuilder.Append($"projectid={projectId}&");
        return this;
    }

    public FilterTasksBuilder WithMyProjects(bool value)
    {
        urlBuilder.Append($"myProjects={value}&");
        return this;
    }

    public FilterTasksBuilder WithMilestone(int milestoneId)
    {
        urlBuilder.Append($"milestoneid={milestoneId}&");
        return this;
    }

    public FilterTasksBuilder WithMyMilestones(bool value)
    {
        urlBuilder.Append($"myMilestones={value}&");
        return this;
    }

    public FilterTasksBuilder WithNoMilestone(bool value)
    {
        urlBuilder.Append($"nomilestone={value}&");
        return this;
    }

    public FilterTasksBuilder WithTag(int projectTag)
    {
        urlBuilder.Append($"tag={projectTag}&");
        return this;
    }

    public FilterTasksBuilder WithStatus(Status status)
    {
        urlBuilder.Append($"status={(int)status}&");
        return this;
    }

    public FilterTasksBuilder WithSubstatus(int customStatusId)
    {
        urlBuilder.Append($"substatus={customStatusId}&");
        return this;
    }

    public FilterTasksBuilder WithFollowed(bool value)
    {
        urlBuilder.Append($"follow={value}&");
        return this;
    }

    public FilterTasksBuilder WithDepartment(string departmentGuid)
    {
        urlBuilder.Append($"department={departmentGuid}&");
        return this;
    }

    public FilterTasksBuilder WithParticipant(string? participantGuid)
    {
        if (participantGuid is null) return this;
        urlBuilder.Append($"participant={participantGuid}&");
        return this;
    }

    public FilterTasksBuilder WithCreator(string? creatorGuid)
    {
        if (creatorGuid is null) return this;
        urlBuilder.Append($"creator={creatorGuid}&");
        return this;
    }

    public FilterTasksBuilder WithDeadlineStart(DateTime startDate)
    {
        urlBuilder.Append($"deadlineStart={startDate:yyyy-MM-dd}&");
        return this;
    }

    public FilterTasksBuilder WithDeadlineStop(DateTime endDate)
    {
        urlBuilder.Append($"deadlineStop={endDate:yyyy-MM-dd}&");
        return this;
    }

    /// <summary>
    /// Last task ID
    /// </summary>
    /// <param name="lastId"></param>
    /// <returns></returns>
    public FilterTasksBuilder WithLastId(int lastId)
    {
        urlBuilder.Append($"lastid={lastId}&");
        return this;
    }

    /// <summary>
    /// Builds url in a string
    /// </summary>
    /// <returns>Builded url</returns>
    public string Build() => urlBuilder.ToString();
}