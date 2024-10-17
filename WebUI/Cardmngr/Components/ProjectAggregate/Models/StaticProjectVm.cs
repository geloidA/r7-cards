using Cardmngr.Shared.Project;

namespace Cardmngr.Components.ProjectAggregate.Models;

public class StaticProjectVm(ProjectStateDto projectState)
{
    public bool IsCollapsed { get; set; } = true;
    public ProjectStateDto ProjectState { get; init; } = projectState;
}
