using Cardmngr.Shared.Project;

namespace Cardmngr.Components.ProjectAggregate.Vms;

public class StaticProjectVm(IProjectStateVm stateVm)
{
    public bool IsCollapsed { get; set; } = true;
    public IProjectStateVm StateVm { get; init; } = stateVm;
}
