using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardLoot
{
    Dirt,
    Rock,
    Stick,
    Demo,
    Glue,
    Double,
    Stun,
    Steal,
    Reverse
}

public enum CardType
{
    Resource,
    Ability
}

[CreateAssetMenu]
public class CardList : ScriptableObject
{
    public List<Card> Cards;
}

[Serializable]
public class Card
{
    public string Name;
    public int ID;
    public GameObject prefab;
    public CardType type;
    public CardLoot loot;
}
