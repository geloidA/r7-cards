using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Project;

public class ProjectStateVm : IProjectStateVm
{
    public Domain.Entities.Project? Project { get; init; }
    public List<OnlyofficeTask> Tasks { get; init; } = [];
    public List<Milestone> Milestones { get; init; } = [];
    public List<OnlyofficeTaskStatus> Statuses { get; init; } = [];
    public List<UserProfile> Team { get; init; } = [];

    Domain.Entities.Project? IProjectStateVm.Project => Project;
    IReadOnlyList<OnlyofficeTask> IProjectStateVm.Tasks => Tasks;
    IReadOnlyList<Milestone> IProjectStateVm.Milestones => Milestones;
    IReadOnlyList<OnlyofficeTaskStatus> IProjectStateVm.Statuses => Statuses;
    IReadOnlyList<UserProfile> IProjectStateVm.Team => Team;
}

public interface IProjectStateVm
{
    Domain.Entities.Project? Project { get; }
    IReadOnlyList<OnlyofficeTask> Tasks { get; }
    IReadOnlyList<Milestone> Milestones { get; }
    IReadOnlyList<OnlyofficeTaskStatus> Statuses { get; }
    IReadOnlyList<UserProfile> Team { get; }
}
