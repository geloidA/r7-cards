using System.Collections;
using System.Collections.Specialized;

namespace BlazorCards.Utils;

public abstract class ObservableLinkedCollection<T> : IEnumerable<T>
{
    protected LinkedList<T> items = [];

    public ObservableLinkedCollection()
    {
        
    }

    public ObservableLinkedCollection(IEnumerable<T> items)
    {
        this.items = new(items);
    }

    public int Count => items.Count;

    public virtual void Add(T item)
    {
        items.AddLast(item);
        OnCollectionChanged();
    }

    public virtual void AddAfter(T target, T item)
    {        
        items.AddAfter(CheckTarget(target), item);
        OnCollectionChanged();
    }

    public virtual void AddBefore(T target, T item)
    {        
        items.AddBefore(CheckTarget(target), item);
        OnCollectionChanged();
    }

    private LinkedListNode<T> CheckTarget(T item) => items.Find(item) ?? throw new InvalidOperationException("Item not found");

    public virtual void Remove(T item)
    {
        items.Remove(item);
        OnCollectionChanged();
    }

    public event Action<NotifyCollectionChangedEventArgs>? CollectionChanged;

    public IEnumerator<T> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void OnCollectionChanged() => CollectionChanged?.Invoke();
}
