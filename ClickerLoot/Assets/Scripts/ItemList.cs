using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemList : ScriptableObject
{
    public List<Item> items = new List<Item>();
}

[Serializable]
public class Item : IComparable<Item>
{
    public string Name;
    public string Description;

    public GameObject item;

    public LootRarity LootRarity;
    public int id;
    public int CompareTo(Item other)
    {


        if (other.LootRarity > LootRarity)
        {
            return -1;
        }
        if (other.LootRarity == LootRarity)
        {
            if (other.id > id) return -1;
            else if (other.id < id) return 1;

            return 0;
        }
        else return 1;
    }
}
