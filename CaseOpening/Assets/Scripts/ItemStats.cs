using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class ItemStats : ScriptableObject
{
    public float maxTotal;
    public List<Item> items;

    private KeyValuePair<float, Item>[] ranges = new KeyValuePair<float, Item>[0];

    public Item GetRandomItem()
    {
        float number = Random.Range(0, maxTotal);

        if (ranges.Length == 0)
            SetRanges();

        float tempNum = 0;

        for (int i = 0; i < ranges.Length; i++)
        {
            tempNum += ranges[i].Key;

            if (number <= tempNum)
            {
                return ranges[i].Value;
            }
        }

        throw new Exception("Couldn't find an item within range");
    }

    private void SetRanges()
    {
        ranges = new KeyValuePair<float, Item>[items.Count];

        for (int i = 0; i < ranges.Length; i++)
        {
            ranges[i] = new KeyValuePair<float, Item>(items[i].percentChance * maxTotal, items[i]);
        }
    }
}

[Serializable]
public class Item
{
    public float percentChance;
    public GameObject itemPrefab;

    // More...
}
