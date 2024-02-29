using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Exceptions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Providers;

using Timer = System.Timers.Timer;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectState : ComponentBase, IAsyncDisposable
{
    private int previousId = -1;
    private ProjectHubClient hubClient = null!;

    [Parameter] public int Id { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private Timer refreshTimer = new();

    public ProjectStateVm? Model { get; set; }

    public event Action? StateChanged;
    private void OnStateChanged() => StateChanged?.Invoke();

    public event Action? MilestonesChanged;
    private void OnMilestonesChanged() => MilestonesChanged?.Invoke();

    public event Action? SelectedMilestonesChanged;
    private void OnSelectedMilestonesChanged() => SelectedMilestonesChanged?.Invoke();

    public event Action? TasksChanged;
    private void OnTasksChanged() => TasksChanged?.Invoke();

    public event Action? SubtasksChanged;
    private void OnSubtasksChanged() => SubtasksChanged?.Invoke();

    /// <summary>
    /// Tasks with selected milestones
    /// </summary>
    public IEnumerable<OnlyofficeTask>? OutputTasks
    {
        get
        {
            return selectedMilestones.Count != 0 ? Model?.Tasks.FilterByMilestones(selectedMilestones) : Model?.Tasks;
        }
    }

    public bool Initialized { get; private set; }

    protected override async Task OnParametersSetAsync()
    {
        Initialized = false;

        if (previousId != Id)
        {
            Model = await ProjectClient.GetProjectAsync(Id);
            selectedMilestones = [];
            previousId = Id;

            hubClient = await GetNewHubClientAsync();
            await hubClient.StartAsync();
            
            refreshTimer.Dispose();
            refreshTimer = new Timer(TimeSpan.FromSeconds(7));
            refreshTimer.Elapsed += async (_, _) =>
            {
                Model = await ProjectClient.GetProjectAsync(Id);
                OnStateChanged();
            };
        }

        Initialized = true;
        refreshTimer.Enabled = true;
    }

    private async Task<ProjectHubClient> GetNewHubClientAsync()
    {
        if (hubClient is { })
        {
            await hubClient.DisposeAsync();
        }

        var client = new ProjectHubClient(NavigationManager, TaskClient, Id, AuthenticationStateProvider.ToCookieProvider());

        client.OnUpdatedTask += UpdateTask;
        client.OnDeletedTask += RemoveTask;
        client.OnCreatedTask += AddTask;

        return client;
    }

    private readonly Stack<object?> BlockRefreshStack = new();

    public void AllowRefresh()
    {
        _ = BlockRefreshStack.TryPop(out var _);

        if (BlockRefreshStack.Count == 0)
        {
            refreshTimer.Enabled = true;
        }
    }

    public void BlockRefresh()
    {
        BlockRefreshStack.Push(null);
        refreshTimer.Enabled = false;
    }

    private List<Milestone> selectedMilestones = [];
    public IReadOnlyList<Milestone> SelectedMilestones => selectedMilestones;

    public void ToggleMilestone(Milestone milestone)
    {
        if (!selectedMilestones.Remove(milestone))
        {
            selectedMilestones.Add(milestone);
        }

        OnSelectedMilestonesChanged();
    }

    public DateTime? Start => Model?.Tasks.Min(x => x.StartDate);
    
    public DateTime? Deadline => Model?.Tasks.Max(x => x.Deadline);

    public DateTime GetMilestoneStart(Milestone milestone)
    {
        var minStart = Model?.Tasks
            .Where(x => x.MilestoneId == milestone.Id)
            .Min(x => x.StartDate);
        
        var defaultStart = milestone.Deadline.AddDays(-7);
        
        return minStart == null || defaultStart < minStart 
            ? defaultStart 
            : minStart.Value;
    }

    public Milestone? GetMilestone(int? milestoneId)
    {
        if (milestoneId == null) return null;

        return Model?.Milestones.FirstOrDefault(x => x.Id == milestoneId);
    }

    public IEnumerable<OnlyofficeTask> GetMilestoneTasks(Milestone milestone)
    {
        return Model?.Tasks.Where(x => x.MilestoneId == milestone.Id) ?? [];
    }

    public IEnumerable<OnlyofficeTask> GetMilestoneTasks(int milestoneId)
    {
        return Model?.Tasks.Where(x => x.MilestoneId == milestoneId) ?? [];
    }

    internal void UpdateTask(OnlyofficeTask task)
    {
        Model!.Tasks.RemoveSingle(x => x.Id == task.Id);

        Model.Tasks.Add(task);

        OnTasksChanged();
    }

    internal void AddTask(OnlyofficeTask created)
    {
        Model!.Tasks.Add(created);
        
        OnTasksChanged();
    }

    internal void RemoveTask(int taskId)
    {
        Model!.Tasks.RemoveSingle(x => x.Id == taskId);

        OnTasksChanged();       
    }

    internal void AddMilestone(Milestone milestone)
    {
        Model!.Milestones.Add(milestone);
        
        OnMilestonesChanged();
    }

    internal void RemoveMilestone(int milestoneId)
    {
        RemoveTaskMilestoneIds(GetMilestoneTasks(milestoneId).ToList());
        Model!.Milestones.RemoveSingle(x => x.Id == milestoneId);

        OnMilestonesChanged();
    }

    private void RemoveTaskMilestoneIds(List<OnlyofficeTask> tasks)
    {
        Model!.Tasks.RemoveAll(tasks.Contains);
        tasks.ForEach(x => Model.Tasks.Add(x with { MilestoneId = null }));
    }

    internal void UpdateMilestone(Milestone milestone)
    {
        Model!.Milestones.RemoveSingle(x => x.Id == milestone.Id);
        Model.Milestones.Add(milestone);

        OnMilestonesChanged();
    }

    internal void AddSubtask(int taskId, Subtask subtask)
    {
        Model!.Tasks.Single(x => x.Id == taskId).Subtasks.Add(subtask);
        OnSubtasksChanged();
    }

    internal void RemoveSubtask(int taskId, int subtaskId)
    {
        Model!.Tasks.Single(x => x.Id == taskId).Subtasks.RemoveSingle(x => x.Id == subtaskId);
        OnSubtasksChanged();
    }

    internal void UpdateSubtask(int taskId, Subtask subtask)
    {
        var task = Model!.Tasks.Single(x => x.Id == taskId);
        task.Subtasks.RemoveSingle(x => x.Id == subtask.Id);
        task.Subtasks.Add(subtask);
        OnSubtasksChanged();
    }

    public async ValueTask DisposeAsync()
    {
        if (hubClient is { })
        {
            await hubClient.DisposeAsync();
        }

        refreshTimer.Dispose();
    }
}
