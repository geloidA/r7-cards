using Cardmngr.Components.ProjectAggregate.Contracts;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;

namespace Cardmngr.Utils;

public interface IProjectStateFinder
{
    IProjectState Find(Milestone milestone);

    IProjectState Find(Project project);

    IProjectState Find(OnlyofficeTask task);

    IReadOnlyList<IProjectState> States { get; }

    event Action<StateChanged> StateChanged;
}
