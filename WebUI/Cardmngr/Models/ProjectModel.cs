using Cardmngr.Models;
using Cardmngr.Models.Base;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Models;
using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr;

public class ProjectModel : ProjectModelBase
{
    private readonly MilestoneTimelineModel milestoneTimeline;
    private readonly StatusColumnBoard statusColumns;
    private readonly List<IUser> team;

    public ProjectModel(Project project, List<MyTask> tasks, List<MyTaskStatus> statuses, List<MilestoneDto> milestones, IEnumerable<IUser> team)
        : base(project)
    {
        milestoneTimeline = new MilestoneTimelineModel(milestones, this);        
        milestoneTimeline.SelectedMilestonesChanged += OnModelChanged;

        this.team = team.ToList();
        statusColumns = new StatusColumnBoard(statuses, tasks, this);
    }

    public override IStatusColumnBoard StatusBoard => statusColumns;

    public override IEnumerable<IUser> Team
    {
        get
        {
            foreach (var user in team)
                yield return user;
        }
    }

    public override IObservableCollection<IMilestoneModel> Milestones => milestoneTimeline;

    public bool IsSelected(IMilestoneModel milestone) => milestoneTimeline.IsSelected(milestone);

    public void ToggleMilestone(IMilestoneModel milestone) => milestoneTimeline.ToggleMilestone(milestone);

    public IEnumerable<IMilestoneModel> SelectedMilestones => milestoneTimeline.SelectedMilestones;
}
