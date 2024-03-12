using System.Collections.Concurrent;

namespace Cardmngr.Server.Hubs;

public abstract class ClientsManager<TKey, TValue> 
    where TKey : notnull 
    where TValue : notnull
{    
    private readonly ConcurrentDictionary<TKey, TValue> keyValuePairs = [];

    public IEnumerable<KeyValuePair<TKey, TValue>> ValuePairs
    {
        get
        {
            foreach (var pair in keyValuePairs)
                yield return pair;
        }
    }

    public int Count => keyValuePairs.Count;

    public bool ContainsKey(TKey key) => keyValuePairs.ContainsKey(key);

    public void Add(TKey key, TValue value)
    {
        keyValuePairs.TryAdd(key, value);
    }

    public bool Remove(TKey key)
    {
        if (keyValuePairs.TryRemove(key, out var value))
        {
            return true;
        }
        return false;
    }

    public TValue this[TKey key]
    {
        get
        {
            return keyValuePairs[key];
        }

        set
        {
            keyValuePairs[key] = value;
        }
    }

    public bool TryGet(TKey key, out TValue? data) => keyValuePairs.TryGetValue(key, out data);
}