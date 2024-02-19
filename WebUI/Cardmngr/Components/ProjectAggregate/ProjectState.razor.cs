using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.Milestone;
using Cardmngr.Application.Clients.Subtask;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Extensions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models;
using System.Threading.Tasks;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectState : ComponentBase
{
    private int previousId = -1;

    [Parameter] public int Id { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] public IProjectClient ProjectClient { get; set; } = null!;

    [Inject] public ITaskClient TaskClient { get; set; } = null!;

    [Inject] public IMilestoneClient MilestoneClient { get; set; } = null!;

    [Inject] public ISubtaskClient SubtaskClient { get; set; } = null!;

    public ProjectStateVm? Model { get; set; }

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
    
    public DateTime? Deadline => Model?.Tasks.Max(x => x.Deadline);

    public DateTime GetMilestoneStart(Milestone milestone)
    {
        var minStart = Model?.Tasks
            .Where(x => x.MilestoneId == milestone.Id)
            .Min(x => x.StartDate);
        
        return minStart ?? milestone.Deadline.AddDays(-7);
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

    internal async Task UpdateTaskStatusAsync(OnlyofficeTask task, OnlyofficeTaskStatus status)
    {
        if (!task.HasStatus(status))
        {
            var updated = await TaskClient.UpdateTaskStatusAsync(task.Id, status);

            Model!.Tasks.RemoveAll(x => x.Id == task.Id);
            Model.Tasks.Add(updated);
        }
    }

    internal async Task AddTaskAsync(TaskUpdateData task)
    {
        var created = await TaskClient.CreateAsync(Model!.Project.Id, task);
        Model.Tasks.Add(created);
        
        StateHasChanged();
    }

    internal async Task RemoveTaskAsync(int taskId)
    {
        await TaskClient.RemoveAsync(taskId);
        Model!.Tasks.RemoveAll(x => x.Id == taskId);

        StateHasChanged();
    }

    internal async Task UpdateTaskAsync(int taskId, TaskUpdateData task)
    {
        var updated = await TaskClient.UpdateAsync(taskId, task);
        Model!.Tasks.RemoveAll(x => x.Id == taskId);
        Model.Tasks.Add(updated);

        StateHasChanged();
    }

    internal async Task AddMilestoneAsync(MilestoneUpdateData milestone)
    {
        var added = await MilestoneClient.CreateAsync(Model!.Project.Id, milestone);
        Model.Milestones.Add(added);
        
        StateHasChanged();
    }

    internal async Task RemoveMilestoneAsync(int milestoneId)
    {
        await MilestoneClient.RemoveAsync(milestoneId);
        Model!.Milestones.RemoveAll(x => x.Id == milestoneId);

        StateHasChanged();
    }

    internal async Task UpdateMilestoneAsync(int milestoneId, MilestoneUpdateData milestone)
    {
        var updated = await MilestoneClient.UpdateAsync(milestoneId, milestone);
        Model!.Milestones.RemoveAll(x => x.Id == milestoneId);
        Model!.Milestones.Add(updated);

        StateHasChanged();
    }

    internal async Task AddSubtaskAsync(int taskId, SubtaskUpdateData subtask)
    {
        var added = await SubtaskClient.CreateAsync(taskId, subtask);
        Model!.Tasks.Single(x => x.Id == taskId).Subtasks.Add(added);
    }

    internal async Task RemoveSubtaskAsync(int taskId, int subtaskId)
    {
        await SubtaskClient.RemoveAsync(taskId, subtaskId);
        Model!.Tasks.Single(x => x.Id == taskId).Subtasks.RemoveAll(x => x.Id == subtaskId);
    }

    internal async Task UpdateSubtaskAsync(int taskId, int subtaskId, SubtaskUpdateData updateData)
    {
        var updated = await SubtaskClient.UpdateAsync(taskId, subtaskId, updateData);
        var task = Model!.Tasks.Single(x => x.Id == taskId);
        task.Subtasks.RemoveAll(x => x.Id == subtaskId);
        task.Subtasks.Add(updated);
    }

    internal async Task UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status)
    {
        var updated = await SubtaskClient.UpdateSubtaskStatusAsync(taskId, subtaskId, status);
        var task = Model!.Tasks.Single(x => x.Id == taskId);
        task.Subtasks.RemoveAll(x => x.Id == subtaskId);
        task.Subtasks.Add(updated);
    }
}
