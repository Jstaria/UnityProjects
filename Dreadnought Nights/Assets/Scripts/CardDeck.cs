using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardSuit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

[CreateAssetMenu]
public class CardDeck : ScriptableObject
{
    public List<GameObject> cards;
}
