using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;

namespace Cardmngr.Components.ProjectAggregate;

public interface IProjectState
{
    ProjectStateVm? Model { get; }

    event Action? StateChanged;
    event Action? MilestonesChanged;
    event Action? SelectedMilestonesChanged;
    event Action? TasksChanged;
    event Action? SubtasksChanged;

    bool Initialized { get; }

    IEnumerable<Milestone> SelectedMilestones { get; }

    void AddTask(OnlyofficeTask created);
    void UpdateTask(OnlyofficeTask task);
    void RemoveTask(int taskId);

    void AddMilestone(Milestone milestone);
    void UpdateMilestone(Milestone milestone);
    void RemoveMilestone(int milestoneId);
    void ToggleMilestone(Milestone milestone);

    void AddSubtask(int taskId, Subtask subtask);
    void UpdateSubtask(int taskId, Subtask subtask);
    void RemoveSubtask(int taskId, int subtaskId);
}
