using System.Collections;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

public class MilestoneTimelineModel(IEnumerable<Milestone> milestones, ProjectModel project) : ModelBase, IEnumerable<MilestoneModel>
{
    private readonly HashSet<MilestoneModel> milestones = milestones
        .Select(m => new MilestoneModel(m, project))
        .ToHashSet();

    public void AddMilestone(MilestoneModel milestone) 
    {
        if (milestones.Contains(milestone))
            throw new InvalidOperationException("Milestone already exists");
        
        milestones.Add(milestone);

        OnModelChanged();
    }

    public bool DeleteMilestone(MilestoneModel milestone)
    {
        if (milestones.Remove(milestone))
        {
            OnModelChanged();
            return true;
        }

        return false;
    }

    public void ToggleMilestone(MilestoneModel milestone)
    {
        if (!milestones.Contains(milestone)) throw new InvalidOperationException("Milestone does not exist");

        milestone.ToggleSelection(); // TODO: fix

        OnModelChanged();
        SelectedMilestonesChanged?.Invoke();
    }

    public int Count => milestones.Count;
    
    public event Action? SelectedMilestonesChanged;

    public IEnumerable<MilestoneModel> SelectedMilestones => milestones.Where(x => x.IsSelected);

    public IEnumerator<MilestoneModel> GetEnumerator() => milestones.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
