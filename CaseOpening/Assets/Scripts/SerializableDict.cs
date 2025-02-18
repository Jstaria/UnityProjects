using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class KeyValue<Key, Value>
{
    public Key key;
    public Value value;
}

[Serializable]
public class SerializableDict<Key, Value> : MonoBehaviour
{
    public List<KeyValue<Key, Value>> pairs;
    public Dictionary<Key, Value> dictionary;

    public Value this[Key index]
    {
        get { return dictionary[index]; }
        set { dictionary[index] = value; }
    }

    internal void Add(Key key, Value value)
    {
        if (!ContainsKey(key))
            dictionary.Add(key, value);
    }

    internal bool ContainsKey(Key key)
    {
        foreach (Key item in dictionary.Keys)
        {
            if (key.Equals(item))
            {
                return true;
            }
        }

        return false;
    }

    private void Start()
    {
        for (int i = 0; i < pairs.Count; i++)
        {
            dictionary.Add(pairs[i].key, pairs[i].value);
        }
    }

}
