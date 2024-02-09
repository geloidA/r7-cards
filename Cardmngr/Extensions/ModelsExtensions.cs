using Cardmngr.Models;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Extensions;

public static class ModelsExtensions
{
    public static UpdatedStateTask GetUpdateState(this ITaskModel val)
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
            Progress = val.Progress
        };
    }

    public static UpdatedStateSubtask GetUpdatedState(this ISubtaskModel val)
    {
        return new UpdatedStateSubtask
        {
            Title = val.Title,
            Responsible = val.Responsible?.Id
        };
    }

    public static UpdatedStateMilestone GetUpdatedState(this IMilestoneModel val)
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

    public static IEnumerable<ITaskModel> OrderByProperties(this IEnumerable<ITaskModel> tasks) // TODO: TEST
    {
        return tasks
            .OrderByDescending(x => x.IsDeadlineOut())
            .ThenBy(x => (x.StatusColumn.Status, -(int)x.Priority, x.Deadline ?? DateTime.MaxValue));
    }

    public static IEnumerable<IMilestoneModel> OrderByProperties(this IEnumerable<IMilestoneModel> milestones) // TODO: TEST
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

    public static Status ToMilestoneStatus(this int statusVal) => statusVal == 0 ? Status.Open : Status.Closed;

    public static bool CanMarkClosed(this ITaskModel task) => task.IsClosed() || task.Subtasks.All(x => x.Status == Status.Closed);

    public static void CloseAllSubtasks(this ITaskModel task)
    {
        if (!task.Subtasks.Any())
            throw new InvalidOperationException("No subtasks found");

        foreach (var subtask in task.Subtasks)
        {
            subtask.Close();
        }
    }
}
