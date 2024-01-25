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
}
