using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{
    public bool canSplit { get; private set; }

    private List<GameObject>[] hands = new List<GameObject>[2];

    public List<GameObject>[] Hands { get { return hands; } }

    public int[] handValues = new int[2];

    public UnityEvent OnBust;

    public int currentHand = 0;

    public bool isHandEmpty;

    private void Start()
    {
        hands[0] = new List<GameObject>();
    }

    private void Update()
    {
        if (hands == null || hands[0] == null || hands[0].Count < 1) return;

        canSplit = hands[0] != null && hands[0].Count == 2 && hands[0][0].GetComponent<Card>().value == hands[0][1].GetComponent<Card>().value;
    }

    public void Split()
    {
        hands[1] = new List<GameObject>();

        hands[1].Add(hands[0][1]);
        hands[0].RemoveAt(1);
    }

    public void AddCard(int hand, GameObject card)
    {
        isHandEmpty = false;
        hands[hand].Add(card);
    }

    public bool CheckBust()
    {
        return GetHandValue() > 21;
    }

    public int GetHandValue()
    {
        int value = 0;
        int numAces = 0;

        List<int> indices = new List<int>();

        for (int i = 0; i < hands[currentHand].Count; i++)
        {
            Card card = hands[currentHand][i].GetComponent<Card>();

            if (card.isAce && card.value != 1)
            {
                numAces++;
                indices.Add(i);
            } 

            value += card.value;
        }

        if (value > 21 && numAces > 0)
        {
            while (numAces > 0 || value > 21)
            {
                value -= 10;
                hands[currentHand][indices[numAces - 1]].GetComponent<Card>().value = 1;
                indices.RemoveAt(numAces - 1);
                numAces--;
            }
            
        }

        handValues[currentHand] = value;

        return value;
    }

    public void StartSweepHand()
    {
        StartCoroutine(SweepAwayCards());
    }

    public IEnumerator DeleteHand()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < hands.Length; i++)
        {
            if (hands[i] == null) break;

            for (int j = 0; j < hands[i].Count; j++)
            {
                // temp code, first sweep them away 
                GameObject.Destroy(hands[i][j]);
            }

            hands[i].Clear();
        }

        isHandEmpty = true;
    }

    private IEnumerator SweepAwayCards()
    {
        if (hands[0].Count < 1) yield return null;

        Vector3 position = hands[0][0].transform.position;
        Quaternion rotation = hands[0][0].transform.rotation;

        for (int i = 0; i < hands.Length; i++)
        {
            if (hands[i] == null) break;

            for (int j = 0; j < hands[i].Count; j++)
            {
                // temp code, first sweep them away 

                Card card = hands[i][j].GetComponent<Card>();

                // If there is clipping, its bc of this
                position.y = card.transform.position.y;

                StartCoroutine(card.MoveTo(position, rotation, 10, 5));
            }
        }

        yield return new WaitForSeconds(1.5f);
    }
}
