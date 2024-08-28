using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private float chipHeight;

    public int betAmount;
    private int prevBetAmount;
    public bool allChipsStill;

    public TVScreen tvScreen;

    public float TokenOffsetInStack = .02f;

    private void Start()
    {
        chipHeight = chip[0].transform.localScale.z * 2;

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

        for (int i = 0; i < 1; i++)
        {
            AddChip(3);
        }

    }

    private void AddChip(int v1)
    {
        bankChips[v1].Add(Instantiate(chip[v1], chipBanks[v1].position + new Vector3(Random.Range(-TokenOffsetInStack, TokenOffsetInStack), chipsInBank[v1] * chipHeight, Random.Range(-TokenOffsetInStack, TokenOffsetInStack)), Quaternion.Euler(90, 0, 0), chipBanks[v1]));
        chipsInBank[v1]++;
    }

    private void MoveChip(List<List<GameObject>> add, List<List<GameObject>> remove, int index, Chip chip, Vector3 position, Transform parent, int chipValue)
    {
        GameObject chipObj = remove[index][remove[index].Count - 1];

        add[index].Add(chipObj);
        remove[index].Remove(chipObj);

        add[index][add[index].Count - 1].GetComponent<Chip>().inBank = !add[index][add[index].Count - 1].GetComponent<Chip>().inBank;
        add[index][add[index].Count - 1].transform.SetParent(parent);

        StartCoroutine(add[index][add[index].Count - 1].GetComponent<Chip>().MoveSequence(position, 5, 15));

        betAmount += chipValue;
    }

    private void BreakChip(List<List<GameObject>> chipList, int removeIndex, int addIndex, Vector3 position, Transform parent, int chipValue)
    {
        int chipAmount = chipValue;

        GameObject.Destroy(chipList[removeIndex][chipList[removeIndex].Count - 1]);
        chipList[removeIndex].Remove(chipList[removeIndex][chipList[removeIndex].Count - 1]);

        Chip tempChip = chip[addIndex].GetComponent<Chip>();

        for (int i = 0; i < (int)(chipValue / tempChip.value); i++)
        {
            AddChip(addIndex);
        }
    }

    public void DeleteBetStack()
    {
        for (int i = 0; i < betChips.Count; i++)
        {
            for (int j = 0; j < betChips[i].Count; j++)
            {
                GameObject.Destroy(betChips[i][j]);
            }

            betChips[i].Clear();
        }

        betAmount = 0;
        UpdateText();
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

                    Vector3 position = chipBets[i].position + new Vector3(Random.Range(-TokenOffsetInStack, TokenOffsetInStack), chipsInBets[i] * chipHeight, Random.Range(-TokenOffsetInStack, TokenOffsetInStack));

                    if (chip.CheckMouseClicked() && Input.GetKey(KeyCode.Space))
                    {
                        if (i == 0) continue;

                        position = chipBets[i - 1].position + new Vector3(Random.Range(-TokenOffsetInStack, TokenOffsetInStack), chipsInBets[i - 1] * chipHeight, Random.Range(-TokenOffsetInStack, TokenOffsetInStack));

                        BreakChip(bankChips, i, i - 1, position, chipBets[i - 1], chip.value);
                    }
                    else if (chip.CheckMouseClicked())
                    {
                        MoveChip(betChips, bankChips, i, chip, position, chipBets[i], chip.value);
                    }
                }
            }
            for (int i = 0; i < betChips.Count; i++)
            {
                for (int j = 0; j < betChips[i].Count; j++)
                {
                    Chip chip = betChips[i][j].GetComponent<Chip>();

                    Vector3 position = chipBanks[i].position + new Vector3(Random.Range(-TokenOffsetInStack, TokenOffsetInStack), chipsInBank[i] * chipHeight, Random.Range(-TokenOffsetInStack, TokenOffsetInStack));

                    if (chip.CheckMouseClicked())
                    {
                        MoveChip(bankChips, betChips, i, chip, position, chipBanks[i], -chip.value);
                    }
                }
            }

            for (int i = 0; i < betChips.Count; i++)
            {
                chipsInBank[i] = bankChips[i].Count;
                chipsInBets[i] = betChips[i].Count;
            }
            allChipsStill = true;

            foreach (List<GameObject> chipList in betChips)
            {
                foreach (GameObject chip in chipList)
                {
                    if (!chip.GetComponent<Chip>().isStationary) allChipsStill = false;
                }
            }

            foreach (List<GameObject> chipList in bankChips)
            {
                foreach (GameObject chip in chipList)
                {
                    if (!chip.GetComponent<Chip>().isStationary) allChipsStill = false;
                }
            }

            UpdateText();

            yield return new WaitForNextFrameUnit();
        }
    }

    public void AddBetWin(WinState winState)
    {
        int winAmount = 0;

        switch (winState)
        {
            case WinState.BlackJack:
                winAmount = betAmount + (int)(betAmount * 1.5f);
                break;

            case WinState.Won:
                winAmount = betAmount * 2;
                break;

            case WinState.Tied:
                winAmount = betAmount;
                break;
        }


        while (winAmount > 0)
        {
            for (int i = chip.Count - 1; i > -1; i--)
            {
                int value = chip[i].GetComponent<Chip>().value;
                if (winAmount >= value)
                {
                    AddChip(i);
                    winAmount -= value;
                    break;
                }
            }

            if (winAmount < 10 && winAmount != 0)
            {
                AddChip(0);
                winAmount = 0;

            }
        }
    }

    private void UpdateText()
    {
        if (tvScreen == null) return;

        if (tvScreen.ScreenClicked() && betAmount > 0)
        {
            GlobalVariables.InBettingStage = false;
        }

        if (allChipsStill && !tvScreen.MouseOver())
        {
            string displayText = "BET AMOUNT:\n\n\n" + betAmount;

            bool justFocusedOnScreen = !tvScreen.mouseOver && tvScreen.prevMouseOver;

            if (betAmount != prevBetAmount || justFocusedOnScreen)
            {
                tvScreen.SetText(displayText, 30, justFocusedOnScreen ? 0 : 14);
            }

            prevBetAmount = betAmount;
        }
    }
}
