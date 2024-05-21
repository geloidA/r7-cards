using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Project;

public class ProjectStateDto
{
    public required Domain.Entities.Project Project { get; init; }
    public List<OnlyofficeTask> Tasks { get; init; } = [];
    public List<Milestone> Milestones { get; init; } = [];
    public List<OnlyofficeTaskStatus> Statuses { get; init; } = [];
    public List<UserProfile> Team { get; init; } = [];
}
