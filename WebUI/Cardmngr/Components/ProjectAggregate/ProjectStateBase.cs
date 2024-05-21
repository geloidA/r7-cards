using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Components.Common;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public abstract class ProjectStateBase : ComponentBase, IProjectState
{
    private Project _project = null!;
    private List<UserProfile> _team = [];
    private List<Milestone> _milestones = [];
    private List<OnlyofficeTaskStatus> _statuses = [];
    private List<OnlyofficeTask> _tasks = [];
    private readonly Dictionary<int, List<TaskTag>> _taskTags = [];

    public async Task SetModelAsync(ProjectStateDto model, bool initTags = false, bool cleanCache = true)
    {
        _project = model.Project;
        _team = model.Team;
        _statuses = model.Statuses;
        _milestones = model.Milestones;

        if (cleanCache) _taskTags.Clear();

        if (initTags)
        {
            await InitializeTaskTagsAsync(model.Tasks);
        }
        else
        {
            model.Tasks.ForEach(x => 
            {
                if (_taskTags.TryGetValue(x.Id, out _))
                {
                    x.Tags = _taskTags[x.Id];
                }
            });
        }

        _tasks = model.Tasks;

        OnMilestonesChanged();
        OnTasksChanged();
    }

    public Project Project => _project;
    public IReadOnlyList<OnlyofficeTask> Tasks => _tasks;
    public IReadOnlyList<Milestone> Milestones => _milestones;
    public IReadOnlyList<UserProfile> Team => _team;
    public IReadOnlyList<OnlyofficeTaskStatus> Statuses => _statuses;

    public bool Initialized { get; protected set; }

    public abstract IFilterManager<OnlyofficeTask> TaskFilter { get; }
    
    public event Action? MilestonesChanged;
    protected void OnMilestonesChanged() => MilestonesChanged?.Invoke();

    public event Action? TasksChanged;
    public void OnTasksChanged() => TasksChanged?.Invoke();

    public event Action? SubtasksChanged;
    protected void OnSubtasksChanged() => SubtasksChanged?.Invoke();

    public void UpdateTask(OnlyofficeTask task)
    {
        _tasks.RemoveSingle(x => x.Id == task.Id);
        task.Tags = _taskTags.TryGetValue(task.Id, out var tags) ? tags : [];
        _tasks.Add(task);

        OnTasksChanged();
    }

    public void AddTask(OnlyofficeTask created)
    {
        _tasks.Add(created);

        OnTasksChanged();
    }

    public void RemoveTask(OnlyofficeTask task) => RemoveTask(task.Id);

    public void RemoveTask(int taskId)
    {
        _tasks.RemoveSingle(x => x.Id == taskId);
        _taskTags.Remove(taskId);

        OnTasksChanged();
    }

    public void AddMilestone(Milestone milestone)
    {
        _milestones.Add(milestone);        
        OnMilestonesChanged();
    }

    public void RemoveMilestone(Milestone milestone)
    {
        RemoveTaskMilestoneIds(this.GetMilestoneTasks(milestone.Id).ToList());
        _milestones.RemoveSingle(x => x.Id == milestone.Id);
        
        if (TaskFilter.Filters.SingleOrDefault(x => x is MilestoneTaskFilter) is MilestoneTaskFilter filter && filter.Remove(milestone))
        {
            OnTasksChanged();
        }

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

    protected ProgressState Progress { get; set; } = new();

    private async Task InitializeTaskTagsAsync(List<OnlyofficeTask> tasks)
    {
        Progress.Max = tasks.Count;
        foreach (var task in tasks)
        {
            if (!_taskTags.TryGetValue(task.Id, out _))
            {
                var value = await TaskClient.GetTaskTagsAsync(task.Id).ToListAsync();
                _taskTags[task.Id] = value;
            }

            task.Tags = _taskTags[task.Id];
            Progress.Value++;
            StateHasChanged();
        }

        Progress.Value = 0;
        await Task.Delay(150); // wait for progress ring animation
    }
}