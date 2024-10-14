using Cardmngr.Components.ProjectAggregate.States;

namespace Cardmngr.Components.ProjectAggregate.Contracts;

public class ProjectStateChanged
{
    public IProjectState? State { get; set; }
}
