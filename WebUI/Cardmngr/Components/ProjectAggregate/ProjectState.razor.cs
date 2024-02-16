using Cardmngr.Application.Clients;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectState : ComponentBase
{
    private int previousId = -1;

    [Parameter] public int Id { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] public IProjectClient ProjectClient { get; set; } = null!;

    public ProjectStateVm? Model { get; set; }
    public bool Initialized { get; private set; }

    protected override async Task OnParametersSetAsync()
    {
        Initialized = false;
        if (previousId != Id)
        {
            Model = await ProjectClient.GetProjectAsync(Id);
            previousId = Id;
        }
        Initialized = true;
    }

    private readonly List<Milestone> selectedMilestones = [];
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
        
    }
}
