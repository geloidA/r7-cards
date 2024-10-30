using Cardmngr.Components.ProjectAggregate.States;

namespace Cardmngr.Components.ProjectAggregate.Contracts;

public class StateChanged
{
    public required IProjectState State { get; init; }
}
