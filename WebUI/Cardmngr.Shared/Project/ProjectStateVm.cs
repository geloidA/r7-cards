using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Entities.Base;

namespace Cardmngr.Shared.Project;

public class ProjectStateDto : IProjectStateViewer
{
    public required Domain.Entities.Project Project { get; init; }
    public List<OnlyofficeTask> Tasks { get; init; } = [];
    public List<Milestone> Milestones { get; init; } = [];
    public List<OnlyofficeTaskStatus> Statuses { get; init; } = [];
    public List<UserProfile> Team { get; init; } = [];

    IReadOnlyList<OnlyofficeTaskStatus> IProjectStateViewer.Statuses => Statuses;

    IReadOnlyList<UserProfile> IProjectStateViewer.Team => Team;

    IReadOnlyList<OnlyofficeTask> IProjectStateViewer.Tasks => Tasks;

    IReadOnlyList<Milestone> IProjectStateViewer.Milestones => Milestones;
}
