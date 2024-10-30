using BlazorComponentBus;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Components.ProjectAggregate.Contracts;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Components.TaskAggregate.Contracts;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;

namespace Cardmngr.Components.ProjectAggregate.States;

public class ProjectStateBase(bool isReadOnly = false) : IProjectState
{
    private List<UserProfile> _team = [];
    private List<Milestone> _milestones = [];
    private List<OnlyofficeTaskStatus> _statuses = [];
    private List<OnlyofficeTask> _tasks = [];
    private readonly Dictionary<int, List<TaskTag>> _tagsByTaskId = [];

    public void SetModel(ProjectStateDto model)
    {        
        _team = model.Team;
        Project = model.Project;
        _statuses = model.Statuses;
        _milestones = model.Milestones;
        _tasks = model.Tasks;

        foreach (var task in _tasks)
        {
            if (_tagsByTaskId.TryGetValue(task.Id, out var tags))
            {
                task.Tags = tags;
            }
        }

        OnMilestonesChanged();
        OnTasksChanged();
        OnStateChanged();
    }

    public IComponentBus EventBus { get; } = new ComponentBus();

    public Project Project { get; private set; } = null!;

    public IReadOnlyList<UserProfile> Team => _team;
    public IReadOnlyList<OnlyofficeTask> Tasks => _tasks;
    public IReadOnlyList<Milestone> Milestones => _milestones;
    public IReadOnlyList<OnlyofficeTaskStatus> Statuses => _statuses;

    public bool Initialized { get; set; }

    private void OnStateChanged()
    {
        EventBus.Publish(new StateChanged { State = this });
    }

    public event Action<EntityChangedEventArgs<Milestone>?>? MilestonesChanged;
    private void OnMilestonesChanged(EntityChangedEventArgs<Milestone>? args = null) => MilestonesChanged?.Invoke(args);

    public event Action<EntityChangedEventArgs<OnlyofficeTask>?>? TasksChanged;
    private void OnTasksChanged(EntityActionType actionType, OnlyofficeTask task) 
        => TasksChanged?.Invoke(new EntityChangedEventArgs<OnlyofficeTask>(actionType, task));

    private void OnTasksChanged() => TasksChanged?.Invoke(null);

    public event Action? SubtasksChanged;
    private void OnSubtasksChanged() => SubtasksChanged?.Invoke();

    public void UpdateTask(OnlyofficeTask task) => UpdateTask(task, EntityActionType.Update);
    
    public void ChangeTaskStatus(OnlyofficeTask task)
    {
        UpdateTask(task, EntityActionType.ChangeStatus);
        EventBus.Publish(new TaskStatusChanged { Task = task });
    }

    private void UpdateTask(OnlyofficeTask task, EntityActionType actionType)
    {
        _tasks.RemoveSingle(x => x.Id == task.Id);
        task.Tags = _tagsByTaskId.TryGetValue(task.Id, out var tags) ? tags : [];
        _tasks.Add(task);
        OnTasksChanged(actionType, task);
        OnStateChanged();
    }

    public void AddTask(OnlyofficeTask created)
    {
        _tasks.Add(created);

        if (created.Tags.Count > 0)
        {
            _tagsByTaskId.Add(created.Id, created.Tags);
        }
        OnTasksChanged(EntityActionType.Add, created);
        OnStateChanged();
    }

    public void RemoveTask(OnlyofficeTask task) => RemoveTask(task.Id, task);

    protected void RemoveTask(int taskId, OnlyofficeTask? task = null)
    {
        _tasks.RemoveSingle(x => x.Id == taskId);
        _tagsByTaskId.Remove(taskId);
        OnTasksChanged(EntityActionType.Remove, task ?? new OnlyofficeTask { Id = taskId });
        OnStateChanged();
    }

    public void AddMilestone(Milestone milestone)
    {
        _milestones.Add(milestone);        
        OnMilestonesChanged();
        OnStateChanged();
    }

    public void RemoveMilestone(Milestone milestone)
    {
        RemoveTaskMilestoneIds(this.GetMilestoneTasks(milestone.Id).ToList());
        _milestones.RemoveSingle(x => x.Id == milestone.Id);
        OnMilestonesChanged(new EntityChangedEventArgs<Milestone>(EntityActionType.Remove, milestone));
        OnStateChanged();
    }

    private void RemoveTaskMilestoneIds(List<OnlyofficeTask> milestoneTasks)
    {
        _tasks.RemoveAll(milestoneTasks.Contains);
        milestoneTasks.ForEach(x => _tasks.Add(x with { MilestoneId = null }));
    }

    public void UpdateMilestone(Milestone milestone)
    {
        _milestones.RemoveSingle(x => x.Id == milestone.Id);
        _milestones.Add(milestone);
        OnMilestonesChanged(new EntityChangedEventArgs<Milestone>(EntityActionType.Update, milestone));
        OnStateChanged();
    }

    public void AddSubtask(int taskId, Subtask subtask)
    {
        _tasks.Single(x => x.Id == taskId).Subtasks.Add(subtask);
        OnSubtasksChanged();
        OnStateChanged();
    }

    public void RemoveSubtask(int taskId, int subtaskId)
    {
        _tasks.Single(x => x.Id == taskId).Subtasks.RemoveSingle(x => x.Id == subtaskId);
        OnSubtasksChanged();
        OnStateChanged();
    }

    public void UpdateSubtask(Subtask subtask)
    {
        var task = _tasks.Single(x => x.Id == subtask.TaskId);
        task.Subtasks.RemoveSingle(x => x.Id == subtask.Id);
        task.Subtasks.Add(subtask);
        OnSubtasksChanged();
        OnStateChanged();
    }

    public bool ReadOnly => isReadOnly;

    public async Task InitializeTaskTagsAsync(ITaskClient taskClient, bool silent = false, CancellationToken cancellationToken = default)
    {
        foreach (var task in _tasks.ToList())
        {
            if (!_tagsByTaskId.TryGetValue(task.Id, out var tags))
            {
                tags = await taskClient.GetTaskTagsAsync(task.Id).ToListAsync(cancellationToken).ConfigureAwait(false);
                _tagsByTaskId[task.Id] = tags;
            }

            cancellationToken.ThrowIfCancellationRequested();

            task.Tags = tags;

            if (!silent)
            {
                OnTasksChanged();
                OnStateChanged();
            }
        }
    }

    public void Clean()
    {
        _tagsByTaskId.Clear();

        TasksChanged = null;
        MilestonesChanged = null;
        SubtasksChanged = null;
    }
}