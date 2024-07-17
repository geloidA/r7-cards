using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public abstract class ProjectStateBase(bool isReadOnly = false) : ComponentBase, IProjectState, IDisposable
{
    private Project _project = null!;
    private List<UserProfile> _team = [];
    private List<Milestone> _milestones = [];
    private List<OnlyofficeTaskStatus> _statuses = [];
    private List<OnlyofficeTask> _tasks = [];
    private readonly Dictionary<int, List<TaskTag>> _tagsByTaskId = [];
    
    protected readonly Dictionary<int, int> _commonHeightByKey = [];
    private CancellationTokenSource? _cts;

    public async Task SetModelAsync(ProjectStateDto model, bool firstRender = false)
    {
        Initialized = false;
        
        _team = model.Team;
        _project = model.Project;
        _statuses = model.Statuses;
        _milestones = model.Milestones;

        if (firstRender)
        {
            _tasks = [];
            Initialized = true;
            StateHasChanged();

            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                await InitializeTaskTagsAsync(model.Tasks, token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            OnMilestonesChanged();
            return;
        }

        foreach (var task in model.Tasks)
        {
            if (_tagsByTaskId.TryGetValue(task.Id, out var tags))
            {
                task.Tags = tags;
            }
        }

        _tasks = model.Tasks;

        Initialized = true;
        OnMilestonesChanged();
        OnTasksChanged();
    }


    public Project Project => _project;
    public IReadOnlyList<UserProfile> Team => _team;
    public IReadOnlyList<OnlyofficeTask> Tasks => _tasks;
    public IReadOnlyList<Milestone> Milestones => _milestones;
    public IReadOnlyList<OnlyofficeTaskStatus> Statuses => _statuses;

    public bool Initialized { get; protected set; }

    public event Action? MilestonesChanged;
    protected void OnMilestonesChanged() => MilestonesChanged?.Invoke();

    public event Action<TaskChangedEventArgs?>? TasksChanged;
    private void OnTasksChanged(TaskAction action, OnlyofficeTask task) 
        => TasksChanged?.Invoke(new TaskChangedEventArgs(action, task));

    private void OnTasksChanged() => TasksChanged?.Invoke(null);

    public event Action? SubtasksChanged;
    protected void OnSubtasksChanged() => SubtasksChanged?.Invoke();

    public void UpdateTask(OnlyofficeTask task) => UpdateTask(task, TaskAction.Update);
    
    public void ChangeTaskStatus(OnlyofficeTask task) => UpdateTask(task, TaskAction.ChangeStatus);

    private void UpdateTask(OnlyofficeTask task, TaskAction action)
    {
        _tasks.RemoveSingle(x => x.Id == task.Id);
        task.Tags = _tagsByTaskId.TryGetValue(task.Id, out var tags) ? tags : [];
        _tasks.Add(task);

        OnTasksChanged(action, task);
    }

    public void AddTask(OnlyofficeTask created)
    {
        _tasks.Add(created);

        if (created.Tags.Count > 0)
        {
            _tagsByTaskId.Add(created.Id, created.Tags);
        }

        OnTasksChanged(TaskAction.Add, created);
    }

    public void RemoveTask(OnlyofficeTask task) => RemoveTask(task.Id, task);

    public void RemoveTask(int taskId, OnlyofficeTask? task = null)
    {
        _tasks.RemoveSingle(x => x.Id == taskId);
        _tagsByTaskId.Remove(taskId);

        OnTasksChanged(TaskAction.Remove, task ?? new OnlyofficeTask { Id = taskId });
    }

    public void AddMilestone(Milestone milestone)
    {
        _milestones.Add(milestone);        
        OnMilestonesChanged();
    }

    public virtual void RemoveMilestone(Milestone milestone)
    {
        RemoveTaskMilestoneIds(this.GetMilestoneTasks(milestone.Id).ToList());
        _milestones.RemoveSingle(x => x.Id == milestone.Id);

        OnMilestonesChanged();
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
        OnMilestonesChanged();
    }

    public void AddSubtask(int taskId, Subtask subtask)
    {
        _tasks.Single(x => x.Id == taskId).Subtasks.Add(subtask);

        OnSubtasksChanged();
    }

    public void RemoveSubtask(int taskId, int subtaskId)
    {
        _tasks.Single(x => x.Id == taskId).Subtasks.RemoveSingle(x => x.Id == subtaskId);

        OnSubtasksChanged();
    }

    public void UpdateSubtask(Subtask subtask)
    {
        var task = _tasks.Single(x => x.Id == subtask.TaskId);
        task.Subtasks.RemoveSingle(x => x.Id == subtask.Id);
        task.Subtasks.Add(subtask);

        OnSubtasksChanged();
    }

    [Inject] ITaskClient TaskClient { get; set; } = null!;

    public bool ReadOnly => isReadOnly;

    private async Task InitializeTaskTagsAsync(List<OnlyofficeTask> tasks, CancellationToken cancellationToken)
    {
        foreach (var task in tasks)
        {
            if (!_tagsByTaskId.TryGetValue(task.Id, out var tags))
            {
                tags = await TaskClient.GetTaskTagsAsync(task.Id).ToListAsync(cancellationToken);
                _tagsByTaskId[task.Id] = tags;
            }

            cancellationToken.ThrowIfCancellationRequested();

            task.Tags = tags;
            _tasks.Add(task);
            OnTasksChanged();
        }
    }

    protected virtual Task CleanPreviousProjectStateAsync()
    {
        _tagsByTaskId.Clear();
        _commonHeightByKey.Clear();

        TasksChanged = null;
        MilestonesChanged = null;
        SubtasksChanged = null;

        return Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        _cts?.Dispose();
    }
}