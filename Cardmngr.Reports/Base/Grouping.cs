namespace Cardmngr.Reports.Base;

internal class Grouping<TKey, TValue>(TKey key, IEnumerable<TValue> items) : IGrouping<TKey, TValue>
{
    private readonly List<TValue> items = [..items];

    public TKey Key => key;

    public IEnumerator<TValue> GetEnumerator() => items.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
