using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models.Interfaces;

public interface IProjectModel : IModel, IWorkContainer
{
    ProjectStatus Status { get; }
    IUser Responsible { get; }
    bool IsPrivate { get; }
    IEnumerable<IUser> Team { get; }
    IStatusColumnBoard StatusBoard { get; }
    IObservableCollection<IMilestoneModel> Milestones { get; }
}

public interface IStatusColumnBoard : IEnumerable<IStatusColumnModel>
{
    ITaskModel? LastDraggedTask { get; }
    void StartDrag(ITaskModel task);
    void UnsetDraggedTask();
}

public interface IMilestoneModel : IModel, IWorkContainer
{
    IProjectModel Project { get; }
    bool IsKey { get; }
    bool IsNotify { get; }
    Status Status { get; }
    IUser? Responsible { get; }
    IEnumerable<ITaskModel> Tasks { get; }
}
