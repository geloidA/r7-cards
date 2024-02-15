using Cardmngr.Models.EventArgs;

namespace Cardmngr.Models.Interfaces;

public interface IObservableCollection<T> : IEnumerable<T>
{
    int Count { get; }

    event Action<CollectionEventArgs<T>>? CollectionChanged;
    void Add(T item);
    bool Remove(T item);
}
