using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TestProject;

public  class ConcurentMultiKeyDictionary<TKeyId, TKeyName, TValue> : IDictionary<(TKeyId id, TKeyName name), TValue>
{
    private readonly object _lockobj = new object();
    private Dictionary<(TKeyId id, TKeyName name), TValue> valuesCollection = new Dictionary<(TKeyId id, TKeyName key), TValue>();
    private Dictionary<TKeyId, Dictionary<TKeyName, TValue>> idCollection = new Dictionary<TKeyId, Dictionary<TKeyName, TValue>>();
    private Dictionary<TKeyName, Dictionary<TKeyId, TValue>> namesCollection = new Dictionary<TKeyName, Dictionary<TKeyId, TValue>>();

    public TValue this[(TKeyId id, TKeyName name) key] { get => ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection)[key]; set => ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection)[key] = value; }

    public ICollection<(TKeyId id, TKeyName name)> Keys
    {
        get
        {
            lock (_lockobj)
            {
                return ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).Keys;
            }
           
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            lock (_lockobj)
            {
                return (((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).Values);
            }
        }
    }

    public int Count
    {
        get
        {
            lock (_lockobj)
            {
                return ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).Count;

            }
        }
    }


    public bool IsReadOnly => ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).IsReadOnly;

    public void Add((TKeyId id, TKeyName name) key, TValue value)
    {
        lock (_lockobj)
        {
            ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).Add(key, value);
            Add(key.id, key.name, value);
        }
    }

    public void Add(KeyValuePair<(TKeyId id, TKeyName name), TValue> item)
    {
        lock (_lockobj)
        {
            ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).Add(item);
            Add(item.Key.id, item.Key.name, item.Value);
        }
    }

    private void Add(TKeyId id, TKeyName name, TValue value)
    {
        lock (_lockobj)
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

    }
    public void Clear()
    {
        lock (_lockobj)
        {
            valuesCollection.Clear();
            namesCollection.Clear();
            valuesCollection.Clear();
        }
        
    }

    public bool Contains(KeyValuePair<(TKeyId id, TKeyName name), TValue> item)
    {
        lock (_lockobj)
        {
            return ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).Contains(item);
        }
    }

    public bool ContainsKey((TKeyId id, TKeyName name) key)
    {
        lock (_lockobj)
        {
            return ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).ContainsKey(key);
        }
    }

    public void CopyTo(KeyValuePair<(TKeyId id, TKeyName name), TValue>[] array, int arrayIndex)
    {
        lock (_lockobj)
            ((ICollection<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<(TKeyId id, TKeyName name), TValue>> GetEnumerator()
    {
        lock (_lockobj)
        {
            return ((IEnumerable<KeyValuePair<(TKeyId id, TKeyName name), TValue>>)valuesCollection).GetEnumerator();
        }
    }

    public bool Remove((TKeyId id, TKeyName name) key)
    {
        lock (_lockobj)
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

    }

    public bool Remove(KeyValuePair<(TKeyId id, TKeyName name), TValue> item)
    {
        lock (_lockobj)
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
    }

    public bool TryGetValue((TKeyId id, TKeyName name) key, out TValue value)
    {
        lock (_lockobj)
        {
            return ((IDictionary<(TKeyId id, TKeyName name), TValue>)valuesCollection).TryGetValue(key, out value);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        lock (_lockobj)
        {
            return ((System.Collections.IEnumerable)valuesCollection).GetEnumerator();
        }
    }
    public IEnumerable<TValue> GetById(TKeyId id)
    {
        lock (_lockobj)
        {
            return idCollection[id].Values;
        }
    }
    public IEnumerable<TValue> GetByName(TKeyName name)
    {
        lock (_lockobj)
        {
            return namesCollection[name].Values;
        }
    }
    

}