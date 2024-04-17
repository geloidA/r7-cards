namespace Cardmngr.Reports.Base;

class Grouping<TKey, TValue>(TKey key, IEnumerable<TValue> items) : IGrouping<TKey, TValue>
{
    private readonly List<TValue> items = new(items);

    public TKey Key => key;

    public IEnumerator<TValue> GetEnumerator() => items.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
