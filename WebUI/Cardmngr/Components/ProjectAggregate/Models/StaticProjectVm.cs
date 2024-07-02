using Cardmngr.Domain.Entities;

namespace Cardmngr.Components.ProjectAggregate.Models;

public class StaticProjectVm(ProjectInfo projectInfo, ICollection<OnlyofficeTask> tasks)
{
    public bool IsCollapsed { get; set; } = true;
    public ProjectInfo ProjectInfo { get; init; } = projectInfo;
    public ICollection<OnlyofficeTask> Tasks { get; init; } = tasks;
}
