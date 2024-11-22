using BlazorComponentBus;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public abstract class ProjectStateComponentBase(bool isReadOnly = false) : ComponentBase, IProjectState
{
    private readonly ProjectStateBase _state = new(isReadOnly);

    public bool ReadOnly => _state.ReadOnly;

    public Project Project => _state.Project;

    public IReadOnlyList<OnlyofficeTaskStatus> Statuses => _state.Statuses;

    public IReadOnlyList<UserProfile> Team => _state.Team;

    public IReadOnlyList<OnlyofficeTask> Tasks => _state.Tasks;

    public IReadOnlyList<Milestone> Milestones => _state.Milestones;

    public IComponentBus EventBus => _state.EventBus;

    public bool Initialized
    {
        get => _state.Initialized;
        set => _state.Initialized = value;
    }

    public event Action<EntityChangedEventArgs<Milestone>?>? MilestonesChanged
    {
        add => _state.MilestonesChanged += value;
        remove => _state.MilestonesChanged -= value;
    }

    public event Action<EntityChangedEventArgs<OnlyofficeTask>?>? TasksChanged
    {
        add => _state.TasksChanged += value;
        remove => _state.TasksChanged -= value;
    }

    public event Action? SubtasksChanged
    {
        add => _state.SubtasksChanged += value;
        remove => _state.SubtasksChanged -= value;
    }

    public void AddMilestone(Milestone milestone)
    {
        _state.AddMilestone(milestone);
    }

    public void AddSubtask(int taskId, Subtask subtask)
    {
        _state.AddSubtask(taskId, subtask);
    }

    public void AddTask(OnlyofficeTask created)
    {
        _state.AddTask(created);
    }

    public void ChangeTaskStatus(OnlyofficeTask task)
    {
        _state.ChangeTaskStatus(task);
    }

    public virtual void RemoveMilestone(Milestone milestoneId)
    {
        _state.RemoveMilestone(milestoneId);
    }

    public void RemoveSubtask(int taskId, int subtaskId)
    {
        _state.RemoveSubtask(taskId, subtaskId);
    }

    public void RemoveTask(OnlyofficeTask taskId)
    {
        _state.RemoveTask(taskId);
    }

    public void UpdateMilestone(Milestone milestone)
    {
        _state.UpdateMilestone(milestone);
    }

    public void UpdateSubtask(Subtask subtask)
    {
        _state.UpdateSubtask(subtask);
    }

    public void UpdateTask(OnlyofficeTask task)
    {
        _state.UpdateTask(task);
    }
    
    protected virtual Task CleanPreviousProjectStateAsync()
    {
        _state.Clean();
        return Task.CompletedTask;
    }

    public void SetModel(ProjectStateDto model)
    {
        _state.SetModel(model);
    }

    public Task InitializeTaskTagsAsync(ITaskClient taskClient, bool silent = false, CancellationToken cancellationToken = default)
    {
        return _state.InitializeTaskTagsAsync(taskClient, silent, cancellationToken);
    }

    public OnlyofficeTaskStatus DefaultStatus(Status status)
    {
        return _state.DefaultStatus(status);
    }
}
