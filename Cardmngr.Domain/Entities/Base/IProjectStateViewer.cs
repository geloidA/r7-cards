namespace Cardmngr.Domain.Entities.Base;

public interface IProjectStateViewer
{
    Project Project { get; }
    IReadOnlyList<OnlyofficeTaskStatus> Statuses { get; }
    IReadOnlyList<UserProfile> Team { get; }
    IReadOnlyList<OnlyofficeTask> Tasks { get; }
    IReadOnlyList<Milestone> Milestones { get; }
}