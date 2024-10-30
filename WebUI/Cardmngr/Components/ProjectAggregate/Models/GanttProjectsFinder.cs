using Cardmngr.Components.ProjectAggregate.Contracts;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Utils;

namespace Cardmngr.Components.ProjectAggregate.Models;

public class GanttProjectsFinder : IProjectStateFinder
{
    private List<IProjectState> states = [];

    public IReadOnlyList<IProjectState> States 
    {
        set
        {
            foreach (var state in states)
            {
                state.EventBus.UnSubscribeFrom<StateChanged>(OnStateChanged);
            }

            states = [.. value];

            foreach (var state in states)
            {
                state.EventBus.SubscribeTo<StateChanged>(OnStateChanged);
            }
        }
    }

    IReadOnlyList<IProjectState> IProjectStateFinder.States => states;

    public event Action<StateChanged>? StateChanged;

    private void OnStateChanged(StateChanged msg) => StateChanged?.Invoke(msg);

    public IProjectState Find(Milestone milestone)
    {
        return states.First(s => s.Project.Id == milestone.ProjectOwner.Id);
    }

    public IProjectState Find(Project project)
    {
        return states.First(s => s.Project.Id == project.Id);
    }

    public IProjectState Find(OnlyofficeTask task)
    {
        return states.First(s => s.Project.Id == task.ProjectOwner.Id);
    }
}
