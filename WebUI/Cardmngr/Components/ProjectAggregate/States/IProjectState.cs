using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Entities.Base;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Utils;

namespace Cardmngr.Components.ProjectAggregate.States;

public interface IProjectState : IProjectStateViewer
{
    event Action<EntityChangedEventArgs<Milestone>?>? MilestonesChanged;
    event Action<EntityChangedEventArgs<OnlyofficeTask>?>? TasksChanged;
    event Action? SubtasksChanged;

    bool ReadOnly { get; }

    void AddTask(OnlyofficeTask created);
    void UpdateTask(OnlyofficeTask task);
    void ChangeTaskStatus(OnlyofficeTask task);
    void RemoveTask(OnlyofficeTask taskId);

    void AddMilestone(Milestone milestone);
    void UpdateMilestone(Milestone milestone);
    void RemoveMilestone(Milestone milestoneId);

    void AddSubtask(int taskId, Subtask subtask);
    void UpdateSubtask(Subtask subtask);
    void RemoveSubtask(int taskId, int subtaskId);
}

public interface IFilterableProjectState : IProjectState
{
    IFilterManager<OnlyofficeTask> TaskFilter { get; }
}

public interface IRefreshableProjectState : IProjectState
{
    RefreshService RefreshService { get; }
}
