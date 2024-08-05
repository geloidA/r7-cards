using System.Collections.Concurrent;

namespace Cardmngr.Server.Hubs;

public abstract class ClientsManager<TKey, TValue> 
    where TKey : notnull 
    where TValue : notnull
{    
    private readonly ConcurrentDictionary<TKey, TValue> keyValuePairs = [];

    protected IEnumerable<KeyValuePair<TKey, TValue>> ValuePairs => keyValuePairs;

    public int Count => keyValuePairs.Count;

    public bool ContainsKey(TKey key) => keyValuePairs.ContainsKey(key);

    public void Add(TKey key, TValue value)
    {
        keyValuePairs.TryAdd(key, value);
    }

    public bool Remove(TKey key)
    {
        return keyValuePairs.TryRemove(key, out _);
    }

    public TValue this[TKey key]
    {
        get => keyValuePairs[key];

        set => keyValuePairs[key] = value;
    }

    public bool TryGet(TKey key, out TValue? data) => keyValuePairs.TryGetValue(key, out data);
}