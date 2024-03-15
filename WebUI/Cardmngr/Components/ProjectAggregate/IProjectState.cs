using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Utils;

namespace Cardmngr.Components.ProjectAggregate;

public interface IProjectState
{
    IProjectStateVm? Model { get; }

    event Action? StateChanged;
    event Action? MilestonesChanged;
    event Action? TasksChanged;
    event Action? SubtasksChanged;

    bool Initialized { get; }

    public void OnTasksChanged();

    IFilterManager<OnlyofficeTask> TaskFilter { get; }

    void AddTask(OnlyofficeTask created);
    void UpdateTask(OnlyofficeTask task);
    void RemoveTask(OnlyofficeTask taskId);

    void AddMilestone(Milestone milestone);
    void UpdateMilestone(Milestone milestone);
    void RemoveMilestone(Milestone milestoneId);

    void AddSubtask(int taskId, Subtask subtask);
    void UpdateSubtask(Subtask subtask);
    void RemoveSubtask(int taskId, int subtaskId);
}

public interface IRefresheableProjectState : IProjectState
{
    RefreshService RefreshService { get; }
}
