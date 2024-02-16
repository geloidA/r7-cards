using Cardmngr.Application.Clients;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Extensions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectState : ComponentBase
{
    private int previousId = -1;

    [Parameter] public int Id { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] public IProjectClient ProjectClient { get; set; } = null!;
    [Inject] public ITaskClient TaskClient { get; set; } = null!;

    public ProjectStateVm? Model { get; set; }

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
        }
        Initialized = true;
    }

    private List<Milestone> selectedMilestones = [];
    public IReadOnlyList<Milestone> SelectedMilestones => selectedMilestones;

    public void ToggleMilestone(Milestone milestone)
    {
        if (!selectedMilestones.Remove(milestone))
        {
            selectedMilestones.Add(milestone);
        }

        StateHasChanged();
    }

    public DateTime? Start => Model?.Tasks.Min(x => x.StartDate);
    
    public DateTime? End => Model?.Tasks.Max(x => x.Deadline);

    public DateTime GetMilestoneStart(Milestone milestone)
    {
        var minStart = Model?.Tasks
            .Where(x => x.MilestoneId == milestone.Id)
            .Min(x => x.StartDate);
        
        return minStart ?? milestone.Deadline.AddDays(-7);
    }

    public Milestone? TaskMilestone(OnlyofficeTask task)
    {
        return Model?.Milestones.FirstOrDefault(x => x.Id == task.MilestoneId);
    }

    internal async Task UpdateTaskStatusAsync(OnlyofficeTask task, OnlyofficeTaskStatus status)
    {
        if (!task.HasStatus(status))
        {
            await TaskClient.UpdateTaskStatusAsync(task.Id, status);

            task.TaskStatusId = status.Id;
            task.Status = status.StatusType.ToStatus();
        }
    }

    internal async Task AddTaskAsync(OnlyofficeTask task)
    {
        var created = await TaskClient.CreateTaskAsync(Model!.Project.Id, task);
        Model.Tasks.Add(created);
    }

    internal async Task RemoveTaskAsync(int taskId)
    {
        await TaskClient.RemoveTaskAsync(taskId);
        Model!.Tasks.RemoveAll(x => x.Id == taskId);
    }

    internal async Task UpdateTaskAsync(OnlyofficeTask task)
    {
        var updated = await TaskClient.UpdateTaskAsync(Model!.Project.Id, task);
        Model.Tasks.RemoveAll(x => x.Id == task.Id);
        Model.Tasks.Add(updated);
    }
}
