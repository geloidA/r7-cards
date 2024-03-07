using System.Collections.Concurrent;

namespace Cardmngr.Server.Hubs;

public abstract class ClientsManager<TKey, TValue> 
    where TKey : notnull 
    where TValue : notnull
{    
    private readonly ConcurrentDictionary<TKey, TValue> keyValuePairs = [];
    private readonly ConcurrentDictionary<TValue, TKey> valueKeyPairs = [];

    public IEnumerable<KeyValuePair<TKey, TValue>> ValuePairs
    {
        get
        {
            foreach (var pair in keyValuePairs)
                yield return pair;
        }
    }

    public bool ContainsValue(TValue val) => valueKeyPairs.ContainsKey(val);

    public bool TryGetKeyByValue(TValue val, out TKey? key)
    {
        var res = valueKeyPairs.TryGetValue(val, out var data);
        key = data;
        return res;
    }

    public void Add(TKey key, TValue value)
    {
        keyValuePairs.TryAdd(key, value);
        valueKeyPairs.TryAdd(value, key);
    }

    public bool Remove(TKey key)
    {
        if (keyValuePairs.TryRemove(key, out var value))
        {
            valueKeyPairs.TryRemove(value, out _);
            return true;
        }
        return false;
    }

    public bool TryGet(TKey key, out TValue? data) => keyValuePairs.TryGetValue(key, out data);
}
