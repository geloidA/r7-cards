using System.Collections;
using Cardmngr.Models.Base;
using Cardmngr.Models.EventArgs;
using Cardmngr.Models.Interfaces;

namespace Cardmngr;

public abstract class ObservableCollectionModel<T> : ModelBase, IObservableCollection<T>
{
    public event Action<CollectionEventArgs<T>>? CollectionChanged;

    protected void OnCollectionChanged(T item, CollectionAction action) => CollectionChanged?.Invoke(new CollectionEventArgs<T>(this, item, action));

    public abstract IEnumerator<T> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public abstract int Count { get; }
    public abstract void Add(T item);
    public abstract bool Remove(T item);
}
