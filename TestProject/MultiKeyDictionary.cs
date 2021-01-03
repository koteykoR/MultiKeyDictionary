using System.Collections.Generic;

public partial class MultiKeyDictionary<TKeyId, TKeyName, TValue> : IDictionary<(TKeyId id, TKeyName name), TValue>
{
    private readonly Dictionary<(TKeyId id, TKeyName name), TValue> valuesCollection = new Dictionary<(TKeyId id, TKeyName key), TValue>();
    private readonly Dictionary<TKeyId, Dictionary<TKeyName, TValue>> idCollection = new Dictionary<TKeyId, Dictionary<TKeyName, TValue>>();
    private readonly Dictionary<TKeyName, Dictionary<TKeyId, TValue>> namesCollection = new Dictionary<TKeyName, Dictionary<TKeyId, TValue>>();

    public IEnumerable<TValue> GetById(TKeyId id)
    {
        return idCollection[id].Values;
    }
    public IEnumerable<TValue> GetByName(TKeyName name)
    {
        return namesCollection[name].Values;
    }

    public TValue this[(TKeyId id, TKeyName name) key] { get => ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection)[key]; set => ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection)[key] = value; }

    public ICollection<(TKeyId id, TKeyName name)> Keys => ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).Keys;

    public ICollection<TValue> Values => ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).Values;

    public int Count => ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).IsReadOnly;

    public void Add((TKeyId id, TKeyName name) key, TValue value)
    {
        ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).Add(key, value);
        AddIdName(key.id, key.name, value);
    }

    public void Add(KeyValuePair<(TKeyId id, TKeyName name), TValue> item)
    {
        ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).Add(item);
        AddIdName(item.Key.id, item.Key.name, item.Value);
    }

    private void AddIdName(TKeyId id, TKeyName name, TValue value)
    {
        if (!idCollection.TryGetValue(id, out Dictionary<TKeyName, TValue> names))
        {
            names = new Dictionary<TKeyName, TValue>();
            idCollection.Add(id, names);
        }
        names[name] = value;

        if (!namesCollection.TryGetValue(name, out var ids))
        {
            ids = new Dictionary<TKeyId, TValue>();
            namesCollection.Add(name, ids);
        }
        ids[id] = value;
    }

    public void Clear()
    {
        valuesCollection.Clear();
        idCollection.Clear();
        namesCollection.Clear();

    }

    public bool Contains(KeyValuePair<(TKeyId id, TKeyName name), TValue> item)
    {
       
        return ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).Contains(item);
    }

    public bool ContainsKey((TKeyId id, TKeyName name) key)
    {
        return ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).ContainsKey(key);
    }


    public IEnumerator<KeyValuePair<(TKeyId id, TKeyName name), TValue>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).GetEnumerator(); //IEnumerator
    }

    public bool Remove((TKeyId id, TKeyName name) key)
    {
      
            if (valuesCollection.TryGetValue(key, out var col))
            {
                valuesCollection.Remove(key);
                namesCollection.Remove(key.name);
                idCollection.Remove(key.id);
                return true;
            }

            else throw new KeyNotFoundException("element with key" + key + "was not found");

    }

    public bool Remove(KeyValuePair<(TKeyId id, TKeyName name), TValue> item)
    {
            if (valuesCollection.TryGetValue(item.Key, out var col))
            {
                var k = item.Key;
                valuesCollection.Remove(k);
                namesCollection.Remove(k.name);
                idCollection.Remove(k.id);
                return true;
            }

            else throw new KeyNotFoundException("element with key" + item.Key + "was not found");
        
    }

    public bool TryGetValue((TKeyId id, TKeyName name) key, out TValue value)
    {
        return ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).TryGetValue(key, out value);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()  //IEnumerable
    {
        return ((System.Collections.IEnumerable)valuesCollection).GetEnumerator();
    }

    public void CopyTo(KeyValuePair<(TKeyId id, TKeyName name), TValue>[] array, int arrayIndex)
    {
        throw new System.NotImplementedException();
    }
}