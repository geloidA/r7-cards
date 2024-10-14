using BlazorComponentBus;
using Cardmngr.Components.ProjectAggregate.Contracts;
using Cardmngr.Components.ProjectAggregate.States;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Components;

public class ProjectStateNotifier : ComponentBase, IDisposable
{
    [CascadingParameter] private IProjectState State { get; set; } = null!;
    [Inject] private IComponentBus Bus { get; set; } = null!;

    public void Dispose()
    {
        Bus.Publish(new ProjectStateChanged { State = null });
    }

    protected override void OnInitialized()
    {
        Bus.Publish(new ProjectStateChanged { State = State });
    }
}
