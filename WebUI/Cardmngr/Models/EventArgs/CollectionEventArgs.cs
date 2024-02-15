namespace Cardmngr.Models.EventArgs;

public class CollectionEventArgs<T>(object? sender, T item, CollectionAction action) : System.EventArgs
{
    public object? Sender { get; } = sender;
    public T Item { get; } = item;
    public CollectionAction Action { get; } = action;
}
