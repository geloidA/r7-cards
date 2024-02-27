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

    public ProjectStateVm? Model { get; set; }

    public event Action? StateChanged;
    private void OnStateChanged() => StateChanged?.Invoke();

    public event Action? MilestonesChanged;
    private void OnMilestonesChanged() => MilestonesChanged?.Invoke();

    public event Action? SelectedMilestonesChanged;
    private void OnSelectedMilestonesChanged() => SelectedMilestonesChanged?.Invoke();

    public event Action? TasksChanged;
    private void OnTasksChanged() => TasksChanged?.Invoke();

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
        }
        Initialized = true;
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

        client.ConnectedMembersChanged += () => Console.WriteLine("Members changed");

        return client;
    }

    private List<Milestone> selectedMilestones = [];
    public IReadOnlyList<Milestone> SelectedMilestones => selectedMilestones;

    public void ToggleMilestone(Milestone milestone)
    {
        if (!selectedMilestones.Remove(milestone))
        {
            selectedMilestones.Add(milestone);
        }

        OnStateChanged();
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

    internal void UpdateTask(OnlyofficeTask task)
    {
        Model!.Tasks.RemoveSingle(x => x.Id == task.Id);

        Model.Tasks.Add(task);

        OnStateChanged();
        OnTasksChanged();
    }

    internal void AddTask(OnlyofficeTask created)
    {
        Model!.Tasks.Add(created);
        
        OnStateChanged();
        OnTasksChanged();
    }

    internal void RemoveTask(int taskId)
    {
        Model!.Tasks.RemoveSingle(x => x.Id == taskId);

        OnStateChanged();
        OnTasksChanged();       
    }

    internal void AddMilestone(Milestone milestone)
    {
        Model!.Milestones.Add(milestone);
        
        OnStateChanged();
        OnMilestonesChanged();
    }

    internal void RemoveMilestone(int milestoneId)
    {
        Model!.Milestones.RemoveSingle(x => x.Id == milestoneId);

        OnStateChanged();
        OnMilestonesChanged();
    }

    internal void UpdateMilestone(Milestone milestone)
    {
        Model!.Milestones.RemoveSingle(x => x.Id == milestone.Id);

        Model.Milestones.Add(milestone);

        OnStateChanged();
        OnMilestonesChanged();
    }

    internal void AddSubtask(int taskId, Subtask subtask)
    {
        Model!.Tasks.Single(x => x.Id == taskId).Subtasks.Add(subtask);

        OnStateChanged();
    }

    internal void RemoveSubtask(int taskId, int subtaskId)
    {
        Model!.Tasks.Single(x => x.Id == taskId).Subtasks.RemoveSingle(x => x.Id == subtaskId);
        OnStateChanged();
    }

    internal void UpdateSubtask(int taskId, Subtask subtask)
    {
        var task = Model!.Tasks.Single(x => x.Id == taskId);
        task.Subtasks.RemoveSingle(x => x.Id == subtask.Id);
        task.Subtasks.Add(subtask);

        OnStateChanged();
    }

    public async ValueTask DisposeAsync()
    {
        if (hubClient is { })
        {
            await hubClient.DisposeAsync();
        }
    }
}
