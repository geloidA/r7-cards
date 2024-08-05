namespace Onlyoffice.Api.Models.Common;

public sealed class TaskFilterBuilder : FilterBuilder
{
    private TaskFilterBuilder() { }
    public static TaskFilterBuilder Instance => new();

    public TaskFilterBuilder ProjectId(int? projectId)
    {
        if (projectId is null) return this;
        Filters["projectId"] = projectId.ToString()!;
        return this;
    }

    public TaskFilterBuilder MyProjects(bool value)
    {
        Filters["myProjects"] = value.ToString();
        return this;
    }

    public TaskFilterBuilder Milestone(int milestoneId)
    {
        Filters["milestone"] = milestoneId.ToString();
        return this;
    }

    public TaskFilterBuilder MyMilestones(bool value)
    {
        Filters["myMilestones"] = value.ToString();
        return this;
    }

    public TaskFilterBuilder NoMilestone(bool value)
    {
        Filters["noMilestone"] = value.ToString();
        return this;
    }

    public TaskFilterBuilder Tag(int projectTag)
    {
        Filters["tag"] = projectTag.ToString();
        return this;
    }

    public TaskFilterBuilder Status(Status status)
    {
        Filters["status"] = ((int)status).ToString();
        return this;
    }

    public TaskFilterBuilder Substatus(int customStatusId)
    {
        Filters["substatus"] = customStatusId.ToString();
        return this;
    }

    public TaskFilterBuilder Followed(bool value)
    {
        Filters["followed"] = value.ToString();
        return this;
    }

    public TaskFilterBuilder Department(string departmentGuid)
    {
        Filters["department"] = departmentGuid;
        return this;
    }

    public TaskFilterBuilder Participant(string? participantGuid)
    {
        if (participantGuid is null) return this;
        Filters["participant"] = participantGuid;
        return this;
    }

    public TaskFilterBuilder Creator(string? creatorGuid)
    {
        if (creatorGuid is null) return this;
        Filters["creator"] = creatorGuid;
        return this;
    }

    /// <summary>
    /// Ignore if null
    /// </summary>
    /// <param name="startDate"></param>
    /// <returns></returns>
    public TaskFilterBuilder DeadlineStart(DateTime? startDate)
    {
        if (startDate is null) return this;
        Filters["deadlineStart"] = startDate.Value.ToString("yyyy-MM-dd");
        return this;
    }

    /// <summary>
    /// Ignore if null
    /// </summary>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public TaskFilterBuilder DeadlineStop(DateTime? endDate)
    {
        if (endDate is null) return this;
        Filters["deadlineStop"] = endDate.Value.ToString("yyyy-MM-dd");
        return this;
    }

    /// <summary>
    /// Last task ID
    /// </summary>
    /// <param name="lastId"></param>
    /// <returns></returns>
    public TaskFilterBuilder LastId(int lastId)
    {
        Filters["lastid"] = lastId.ToString();
        return this;
    }

    /// <summary>
    /// Builds url in a string
    /// </summary>
    /// <returns>Builded url</returns>
    public override string Build()
    {
        if (Filters.ContainsKey("projectId") && Filters.ContainsKey("myProjects"))
            throw new ArgumentException("Filter 'projectId' and 'myProjects' are mutually exclusive");

        if (Filters.ContainsKey("milestone") && Filters.ContainsKey("myMilestones"))
            throw new ArgumentException("Filter 'milestone' and 'myMilestones' are mutually exclusive");

        return base.Build();
    }
}