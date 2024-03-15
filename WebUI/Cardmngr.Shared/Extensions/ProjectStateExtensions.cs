using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;

namespace Cardmngr.Shared.Extensions;

public static class ProjectStateExtensions
{
    public static void RemoveSubtask(this IProjectStateVm state, int taskId, int subtaskId)
    {
        state.Tasks.Single(x => x.Id == taskId).Subtasks.RemoveSingle(x => x.Id == subtaskId);
    }

    public static void AddSubtask(this IProjectStateVm state, int taskId, Subtask subtask)
    {
        state.Tasks.Single(x => x.Id == taskId).Subtasks.Add(subtask);
    }

    public static void UpdateSubtask(this IProjectStateVm state, Subtask subtask)
    {
        var task = state.Tasks.Single(x => x.Id == subtask.TaskId);
        task.Subtasks.RemoveSingle(x => x.Id == subtask.Id);
        task.Subtasks.Add(subtask);
    }
}
