
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public enum LootRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Sona
}

public class LootSystem : MonoBehaviour
{
    [SerializeField] private ItemList items;

    private Dictionary<LootRarity, List<Item>> ItemTable;

    private void Start()
    {
        CreateItemTable();
    }

    public LootRarity PickRarity(int totalWeight, int[] weightValues)
    {
        int randomNum = Random.Range(0, totalWeight + 1);

        LootRarity rarity = LootRarity.Sona;

        for (int i = 0; i < weightValues.Length; i++)
        {
            if (randomNum < weightValues[i])
            {
                rarity = (LootRarity)i;
                break;
            }
        }

        return rarity;
    }

    public Item GetItem(LootRarity rarity)
    {
        return ItemTable[rarity][Random.Range(0, ItemTable[rarity].Count)];
    }

    private void CreateItemTable()
    {
        ItemTable = new Dictionary<LootRarity, List<Item>>();

        for (int i = 0; i < 6; i++)
        {
            ItemTable.Add((LootRarity)i, new List<Item>());
        }

        foreach (Item i in items.items)
        {
            ItemTable[i.LootRarity].Add(i);
        }
    }
}
