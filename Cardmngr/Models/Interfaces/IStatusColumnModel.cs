using Onlyoffice.Api.Common;

namespace Cardmngr.Models.Interfaces;

public interface IStatusColumnModel : IModel, IObservableCollection<ITaskModel>
{
    Status Status { get; }
    bool CanChangeAvailable { get; }
    string? Image { get; }
    string? ImageType { get; }
    string? Color { get; }
    int Order { get; }
    bool IsDefault { get; }
    bool Available { get; }
    IProjectModel Project { get; }
}
