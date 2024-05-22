using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Utils;

namespace Cardmngr.Components.ProjectAggregate;

public interface IProjectState
{
    event Action? MilestonesChanged;
    event Action<TaskChangedEventArgs?>? TasksChanged;
    event Action? SubtasksChanged;

    bool Initialized { get; }

    public void OnTasksChanged();

    IFilterManager<OnlyofficeTask> TaskFilter { get; }

    Project Project { get; }
    IReadOnlyList<OnlyofficeTaskStatus> Statuses { get; }
    IReadOnlyList<UserProfile> Team { get; }
    IReadOnlyList<OnlyofficeTask> Tasks { get; }

    void AddTask(OnlyofficeTask created);
    void UpdateTask(OnlyofficeTask task);
    void ChangeTaskStatus(OnlyofficeTask task);
    void RemoveTask(OnlyofficeTask taskId);

    IReadOnlyList<Milestone> Milestones { get; }

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
