using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public abstract class ProjectStateBase : ComponentBase, IProjectState
{
    private ProjectStateVm? model;
    public IProjectStateVm? Model
    {
        get => model;
        set
        {
            if (value is not ProjectStateVm newVal)
            {
                throw new InvalidCastException("value is not ProjectStateVm");
            }

            model = newVal;
            OnStateChanged();
        }
    }

    public bool Initialized { get; protected set; }

    public abstract IFilterManager<OnlyofficeTask> TaskFilter { get; }

    public event Action? StateChanged;
    protected void OnStateChanged() => StateChanged?.Invoke();
    
    public event Action? MilestonesChanged;
    protected void OnMilestonesChanged() => MilestonesChanged?.Invoke();

    public event Action? TasksChanged;
    public void OnTasksChanged() => TasksChanged?.Invoke();

    public event Action? SubtasksChanged;
    protected void OnSubtasksChanged() => SubtasksChanged?.Invoke();

    public void UpdateTask(OnlyofficeTask task)
    {
        model!.Tasks.RemoveSingle(x => x.Id == task.Id);
        model.Tasks.Add(task);
        OnTasksChanged();
    }

    public void AddTask(OnlyofficeTask created)
    {
        model!.Tasks.Add(created);        
        OnTasksChanged();
    }

    public void RemoveTask(OnlyofficeTask task) => RemoveTask(task.Id);

    public void RemoveTask(int taskId)
    {
        model!.Tasks.RemoveSingle(x => x.Id == taskId);
        OnTasksChanged();   
    }

    public void AddMilestone(Milestone milestone)
    {
        model!.Milestones.Add(milestone);        
        OnMilestonesChanged();
    }

    public void RemoveMilestone(Milestone milestone)
    {
        RemoveTaskMilestoneIds(this.GetMilestoneTasks(milestone.Id).ToList());
        model!.Milestones.RemoveSingle(x => x.Id == milestone.Id);
        
        if (TaskFilter.Filters.SingleOrDefault(x => x is MilestoneTaskFilter) is MilestoneTaskFilter filter && filter.Remove(milestone))
        {
            OnTasksChanged();
        }

        OnMilestonesChanged();
    }

    private void RemoveTaskMilestoneIds(List<OnlyofficeTask> tasks)
    {
        model!.Tasks.RemoveAll(tasks.Contains);
        tasks.ForEach(x => model.Tasks.Add(x with { MilestoneId = null }));
    }

    public void UpdateMilestone(Milestone milestone)
    {
        model!.Milestones.RemoveSingle(x => x.Id == milestone.Id);
        model.Milestones.Add(milestone);
        OnMilestonesChanged();
    }

    public void AddSubtask(int taskId, Subtask subtask)
    {
        Model!.AddSubtask(taskId, subtask);
        OnSubtasksChanged();
    }

    public void RemoveSubtask(int taskId, int subtaskId)
    {
        Model!.RemoveSubtask(taskId, subtaskId);
        OnSubtasksChanged();
    }

    public void UpdateSubtask(Subtask subtask)
    {
        Model!.UpdateSubtask(subtask);
        OnSubtasksChanged();
    }
}
