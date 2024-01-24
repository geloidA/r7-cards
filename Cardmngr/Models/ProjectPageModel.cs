using BlazorCards;
using Onlyoffice.Api.Models;
using BlazorCards.Core.Extensions;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Models;

public class ProjectPageModel(Project project, List<MyTask> tasks, List<MyTaskStatus> statuses, List<Milestone> milestones) : ModelBase
{
    private readonly ProjectModel projectModel = new(project, tasks, statuses, milestones);

    public string ProjectTitle => projectModel.Title;

    public Card? LastDraggedCard { get; private set; }
    public IEnumerable<Card> Cards => board.AllCards();
    public IEnumerable<MilestoneModel> Milestones => projectModel.Milestones;

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
    
    public event Action? SelectedMilestonesChanged;
}