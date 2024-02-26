using Blazored.Modal.Services;

namespace Cardmngr.Utils.DetailsModal;

public class DetailsModalManager
{
    private readonly Stack<IDetailsModal> openedModals = new();

    public void AddModal(IDetailsModal modal)
    {
        ArgumentNullException.ThrowIfNull(modal);

        if (openedModals.Contains(modal, new EqualityModalComparer()))
        {
            throw new InvalidOperationException("Modal already opened");
        }

        openedModals.Push(modal);
    }

    public int Count => openedModals.Count;

    public bool HaveOpenedModals => openedModals.Count != 0;

    public Guid LastModalGuid => HaveOpenedModals ? openedModals.Peek().Guid : Guid.Empty;

    public async Task CloseLastAsync(ModalResult? result = null)
    {
        if (!HaveOpenedModals)
        {
            throw new InvalidOperationException("No opened modal");
        }

        await openedModals.Pop().CloseAsync(result);
    }

    public async Task CloseSpecific(IDetailsModal modal, ModalResult? result = null) // TODO: TEST
    {
        var comparer = new EqualityModalComparer();

        if (!openedModals.Contains(modal, comparer))
        {
            throw new InvalidOperationException("Modal is not opened");
        }

        while (!comparer.Equals(openedModals.Peek(), modal))
        {
            await openedModals.Pop().CloseAsync(result);
        }

        await openedModals.Pop().CloseAsync(result);
    }
}
