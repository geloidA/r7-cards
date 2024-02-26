using Blazored.Modal.Services;

namespace Cardmngr.Utils.DetailsModal;

public interface IDetailsModal
{
    Guid Guid { get; }
    Task CloseAsync(ModalResult? result);
    bool Closed { get; }
}
