using BlazorComponentBus;
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
                state.EventBus.UnSubscribe<StateChanged>(OnStateChanged);
            }

            states = [.. value];

            foreach (var state in states)
            {
                state.EventBus.Subscribe<StateChanged>(OnStateChanged);
            }

            Console.WriteLine(states.Count);
        }
    }

    public event Action? StateChanged;

    private void OnStateChanged(MessageArgs _) => StateChanged?.Invoke();

    public IProjectState Find(Milestone milestone)
    {
        Console.WriteLine(milestone);
        return states.First(s => s.Project.Id == milestone.ProjectOwner.Id);
    }

    public IProjectState Find(Project project)
    {
        return states.First(s => s.Project.Id == project.Id);
    }

    public IProjectState Find(OnlyofficeTask task)
    {
        Console.WriteLine(states.Count);
        return states.First(s => s.Project.Id == task.ProjectOwner.Id);
    }
}
