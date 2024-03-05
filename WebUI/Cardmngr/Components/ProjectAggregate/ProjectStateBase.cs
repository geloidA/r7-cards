using Cardmngr.Domain.Entities;
using Cardmngr.Exceptions;
using Cardmngr.Extensions;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public abstract class ProjectStateBase : ComponentBase, IProjectState
{
    private ProjectStateVm? model;
    public ProjectStateVm? Model
    {
        get => model;
        set
        {
            model = value;
            OnStateChanged();
        }
    }

    public bool Initialized { get; protected set; }

    private readonly List<Milestone> selectedMilestones = [];
    public IEnumerable<Milestone> SelectedMilestones
    {
        get 
        {
            foreach (var milestone in selectedMilestones)
                yield return milestone;
        }
    }

    protected void ClearSelectedMilestones() => selectedMilestones.Clear();  

    public void ToggleMilestone(Milestone milestone)
    {
        if (!selectedMilestones.Remove(milestone))
        {
            selectedMilestones.Add(milestone);
        }

        OnSelectedMilestonesChanged();
    }

    public event Action? StateChanged;
    protected void OnStateChanged() => StateChanged?.Invoke();
    
    public event Action? MilestonesChanged;
    protected void OnMilestonesChanged() => MilestonesChanged?.Invoke();

    public event Action? SelectedMilestonesChanged;
    protected void OnSelectedMilestonesChanged() => SelectedMilestonesChanged?.Invoke();

    public event Action? TasksChanged;
    protected void OnTasksChanged() => TasksChanged?.Invoke();

    public event Action? SubtasksChanged;
    protected void OnSubtasksChanged() => SubtasksChanged?.Invoke();

    public void UpdateTask(OnlyofficeTask task)
    {
        Model!.Tasks.RemoveSingle(x => x.Id == task.Id);

        Model.Tasks.Add(task);

        OnTasksChanged();
    }

    public void AddTask(OnlyofficeTask created)
    {
        Model!.Tasks.Add(created);
        
        OnTasksChanged();
    }

    public void RemoveTask(int taskId)
    {
        Model!.Tasks.RemoveSingle(x => x.Id == taskId);

        OnTasksChanged();       
    }

    public void AddMilestone(Milestone milestone)
    {
        Model!.Milestones.Add(milestone);
        
        OnMilestonesChanged();
    }

    public void RemoveMilestone(int milestoneId)
    {
        RemoveTaskMilestoneIds(this.GetMilestoneTasks(milestoneId).ToList());
        Model!.Milestones.RemoveSingle(x => x.Id == milestoneId);
        
        if (selectedMilestones.RemoveAll(x => x.Id == milestoneId) > 0)
        {
            OnSelectedMilestonesChanged();
        }

        OnMilestonesChanged();
    }

    private void RemoveTaskMilestoneIds(List<OnlyofficeTask> tasks)
    {
        Model!.Tasks.RemoveAll(tasks.Contains);
        tasks.ForEach(x => Model.Tasks.Add(x with { MilestoneId = null }));
    }

    public void UpdateMilestone(Milestone milestone)
    {
        Model!.Milestones.RemoveSingle(x => x.Id == milestone.Id);
        Model.Milestones.Add(milestone);

        OnMilestonesChanged();
    }

    public void AddSubtask(int taskId, Subtask subtask)
    {
        Model!.Tasks.Single(x => x.Id == taskId).Subtasks.Add(subtask);
        OnSubtasksChanged();
    }

    public void RemoveSubtask(int taskId, int subtaskId)
    {
        Model!.Tasks.Single(x => x.Id == taskId).Subtasks.RemoveSingle(x => x.Id == subtaskId);
        OnSubtasksChanged();
    }

    public void UpdateSubtask(int taskId, Subtask subtask)
    {
        var task = Model!.Tasks.Single(x => x.Id == taskId);
        task.Subtasks.RemoveSingle(x => x.Id == subtask.Id);
        task.Subtasks.Add(subtask);
        OnSubtasksChanged();
    }
}
