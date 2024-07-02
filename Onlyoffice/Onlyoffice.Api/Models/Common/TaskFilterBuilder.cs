namespace Onlyoffice.Api.Models.Common;

public sealed class TaskFilterBuilder : FilterBuilder
{
    private TaskFilterBuilder() { }
    public static TaskFilterBuilder Instance => new();

    public TaskFilterBuilder ProjectId(int? projectId)
    {
        if (projectId is null) return this;
        _filters["projectId"] = projectId.ToString()!;
        return this;
    }

    public TaskFilterBuilder MyProjects(bool value)
    {
        _filters["myProjects"] = value.ToString();
        return this;
    }

    public TaskFilterBuilder Milestone(int milestoneId)
    {
        _filters["milestone"] = milestoneId.ToString();
        return this;
    }

    public TaskFilterBuilder MyMilestones(bool value)
    {
        _filters["myMilestones"] = value.ToString();
        return this;
    }

    public TaskFilterBuilder NoMilestone(bool value)
    {
        _filters["noMilestone"] = value.ToString();
        return this;
    }

    public TaskFilterBuilder Tag(int projectTag)
    {
        _filters["tag"] = projectTag.ToString();
        return this;
    }

    public TaskFilterBuilder Status(Status status)
    {
        _filters["status"] = ((int)status).ToString();
        return this;
    }

    public TaskFilterBuilder Substatus(int customStatusId)
    {
        _filters["substatus"] = customStatusId.ToString();
        return this;
    }

    public TaskFilterBuilder Followed(bool value)
    {
        _filters["followed"] = value.ToString();
        return this;
    }

    public TaskFilterBuilder Department(string departmentGuid)
    {
        _filters["department"] = departmentGuid;
        return this;
    }

    public TaskFilterBuilder Participant(string? participantGuid)
    {
        if (participantGuid is null) return this;
        _filters["participant"] = participantGuid;
        return this;
    }

    public TaskFilterBuilder Creator(string? creatorGuid)
    {
        if (creatorGuid is null) return this;
        _filters["creator"] = creatorGuid;
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
        _filters["deadlineStart"] = startDate.Value.ToString("yyyy-MM-dd");
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
        _filters["deadlineStop"] = endDate.Value.ToString("yyyy-MM-dd");
        return this;
    }

    /// <summary>
    /// Last task ID
    /// </summary>
    /// <param name="lastId"></param>
    /// <returns></returns>
    public TaskFilterBuilder LastId(int lastId)
    {
        _filters["lastid"] = lastId.ToString();
        return this;
    }

    /// <summary>
    /// Builds url in a string
    /// </summary>
    /// <returns>Builded url</returns>
    public override string Build()
    {
        if (_filters.ContainsKey("projectId") && _filters.ContainsKey("myProjects"))
            throw new ArgumentException("Filter 'projectId' and 'myProjects' are mutually exclusive");

        if (_filters.ContainsKey("milestone") && _filters.ContainsKey("myMilestones"))
            throw new ArgumentException("Filter 'milestone' and 'myMilestones' are mutually exclusive");

        return base.Build();
    }
}