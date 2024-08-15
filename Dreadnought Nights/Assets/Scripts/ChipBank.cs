using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ChipBank : MonoBehaviour
{
    public List<Transform> chipBets;
    public List<Transform> chipBanks;

    private List<int> chipsInBank;
    private List<int> chipsInBets;

    public List<List<GameObject>> bankChips;
    public List<List<GameObject>> betChips;

    public List<GameObject> chip;

    public float TokenOffsetInStack = .02f;

    private void Start()
    {
        chipsInBank = new List<int>();
        chipsInBets = new List<int>();
        bankChips = new List<List<GameObject>>();
        betChips = new List<List<GameObject>>();

        for (int i = 0; i < chip.Count; i++)
        {
            bankChips.Add(new List<GameObject>());
            betChips.Add(new List<GameObject>());

            chipsInBank.Add(0);
            chipsInBets.Add(0);
        }

        AddChip(0);
        AddChip(1);
        AddChip(2);
        AddChip(3);
        AddChip(0);
        AddChip(1);
        AddChip(2);
        AddChip(3);
        AddChip(0);
        AddChip(1);
        AddChip(2);
        AddChip(3);

    }

    private void AddChip(int v1)
    {
        bankChips[v1].Add(Instantiate(chip[v1], chipBanks[v1].position + new Vector3(Random.Range(-TokenOffsetInStack, TokenOffsetInStack), chipsInBank[0] * .01f, Random.Range(-TokenOffsetInStack, TokenOffsetInStack)), Quaternion.Euler(90, 0, 0), chipBanks[v1]));
        chipsInBank[v1]++;
    }

    private void MoveChip(List<List<GameObject>> add, List<List<GameObject>> remove, int index, Chip chip, Vector3 position, Transform parent)
    {
        if (chip.CheckMouseClicked())
        {
            GameObject chipObj = remove[index][remove[index].Count - 1];

            add[index].Add(chipObj);
            remove[index].Remove(chipObj);

            chipsInBank[index] = bankChips[index].Count;
            chipsInBets[index] = betChips[index].Count;

            add[index][add[index].Count - 1].GetComponent<Chip>().inBank = !add[index][add[index].Count - 1].GetComponent<Chip>().inBank;
            add[index][add[index].Count - 1].transform.SetParent(parent);

            StartCoroutine(add[index][add[index].Count - 1].GetComponent<Chip>().MoveSequence(position, 5, 15));

        }
    }

    public void StartBetting()
    {
        StartCoroutine(Betting());
    }

    private IEnumerator Betting()
    {
        while (GlobalVariables.InBettingStage)
        {
            for (int i = 0; i < bankChips.Count; i++)
            {
                for (int j = 0; j < bankChips[i].Count; j++)
                {
                    Chip chip = bankChips[i][j].GetComponent<Chip>();

                    Vector3 position = chipBets[i].position + new Vector3(Random.Range(-TokenOffsetInStack, TokenOffsetInStack), chipsInBets[i] * .01f, Random.Range(-TokenOffsetInStack, TokenOffsetInStack));

                    MoveChip(betChips, bankChips, i, chip, position, chipBets[i]);
                }
            }
            for (int i = 0; i < betChips.Count; i++)
            {
                for (int j = 0; j < betChips[i].Count; j++)
                {
                    Chip chip = betChips[i][j].GetComponent<Chip>();

                    Vector3 position = chipBanks[i].position + new Vector3(Random.Range(-TokenOffsetInStack, TokenOffsetInStack), chipsInBank[i] * .01f, Random.Range(-TokenOffsetInStack, TokenOffsetInStack));

                    MoveChip(bankChips, betChips, i, chip, position, chipBanks[i]);
                }
            }

            yield return new WaitForNextFrameUnit();
        }
    }
}
