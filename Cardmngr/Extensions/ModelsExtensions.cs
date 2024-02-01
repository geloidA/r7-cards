using Cardmngr.Models;
using Onlyoffice.Api.Models;

namespace Cardmngr.Extensions;

public static class ModelsExtensions
{
    public static UpdatedStateTask GetUpdateState(this TaskModel val)
    {
        return new UpdatedStateTask
        {
            Description = val.Description,
            Deadline = val.Deadline,
            StartDate = val.StartDate,
            Priority = (int)val.Priority,
            Title = val.Title,
            MilestoneId = val.Milestone?.Id ?? 0,
            Responsibles = val.Responsibles?.Select(x => x.Id).ToList(),
            ProjectId = val.ProjectOwner.Id,
            Progress = val.Progress
        };
    }

    public static UpdatedStateSubtask GetUpdatedState(this SubtaskModel val)
    {
        return new UpdatedStateSubtask
        {
            Title = val.Title,
            Responsible = val.Responsible?.Id
        };
    }

    public static UpdatedStateMilestone GetUpdatedState(this MilestoneModel val)
    {
        return new UpdatedStateMilestone
        {
            Description = val.Description,
            Title = val.Title,
            IsKey = val.IsKey,
            Status = (int)val.Status,
            IsNotify = val.IsNotify,
            Deadline = val.Deadline,
            ProjectID = val.Project.Id,
            Responsible = val.Responsible?.Id,
            NotifyResponsible = val.IsNotify
        };
    }

    public static IEnumerable<TaskModel> OrderByProperties(this IEnumerable<TaskModel> tasks) // TODO: TEST
    {
        return tasks
            .OrderByDescending(x => x.IsDeadlineOut())
            .ThenBy(x => (x.StatusColumn.StatusType, -(int)x.Priority, x.Deadline ?? DateTime.MaxValue));
    }

    public static IEnumerable<MilestoneModel> OrderByProperties(this IEnumerable<MilestoneModel> milestones) // TODO: TEST
    {
        return milestones
            .OrderByDescending(x => x.IsDeadlineOut())
            .ThenBy(x => x.Deadline ?? DateTime.MaxValue);
    }

    public static DateTime CheckStart(this IWork work)
    {
        return work.StartDate ?? throw new NullReferenceException("Work's StartDate is null");
    }

    public static DateTime CheckDeadline(this IWork work)
    {
        return work.Deadline ?? throw new NullReferenceException("Work's Deadline is null");
    }
}
