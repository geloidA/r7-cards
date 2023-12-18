namespace BlazorCards;

public interface IContainer<TItem>
{
    IEnumerable<TItem> Items { get; }
    int Count { get; }
    void Add(TItem item);
    void Remove(TItem item);
}
