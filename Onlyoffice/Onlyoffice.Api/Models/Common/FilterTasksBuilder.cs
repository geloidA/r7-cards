namespace Onlyoffice.Api.Common;

public sealed class FilterTasksBuilder : FilterBuilder
{    
    private FilterTasksBuilder() { }
    public static FilterTasksBuilder Instance => new();

    public FilterTasksBuilder ProjectId(int? projectId)
    {
        if (projectId is null) return this;
        _filters["projectId"] = projectId.ToString()!;
        return this;
    }

    public FilterTasksBuilder MyProjects(bool value)
    {
        _filters["myProjects"] = value.ToString();
        return this;
    }

    public FilterTasksBuilder Milestone(int milestoneId)
    {
        _filters["milestone"] = milestoneId.ToString();
        return this;
    }

    public FilterTasksBuilder MyMilestones(bool value)
    {
        _filters["myMilestones"] = value.ToString();
        return this;
    }

    public FilterTasksBuilder NoMilestone(bool value)
    {
        _filters["noMilestone"] = value.ToString();
        return this;
    }

    public FilterTasksBuilder Tag(int projectTag)
    {
        _filters["tag"] = projectTag.ToString();
        return this;
    }

    public FilterTasksBuilder Status(Status status)
    {
        _filters["status"] = ((int)status).ToString();
        return this;
    }

    public FilterTasksBuilder Substatus(int customStatusId)
    {
        _filters["substatus"] = customStatusId.ToString();
        return this;
    }

    public FilterTasksBuilder Followed(bool value)
    {
        _filters["followed"] = value.ToString();
        return this;
    }

    public FilterTasksBuilder Department(string departmentGuid)
    {
        _filters["department"] = departmentGuid;
        return this;
    }

    public FilterTasksBuilder Participant(string? participantGuid)
    {
        if (participantGuid is null) return this;
        _filters["participant"] = participantGuid;
        return this;
    }

    public FilterTasksBuilder Creator(string? creatorGuid)
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
    public FilterTasksBuilder DeadlineStart(DateTime? startDate)
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
    public FilterTasksBuilder DeadlineStop(DateTime? endDate)
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
    public FilterTasksBuilder LastId(int lastId)
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
        
        return $"filter/{string.Join("&", _filters.Select(x => $"{x.Key}={x.Value}"))}";
    }

    public FilterTasksBuilder Department(object value)
    {
        throw new NotImplementedException();
    }
}