using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : IPlayer, IBet
{
    public Hand dealersHand;

    public void StartTurn()
    {
        StartCoroutine(StartMakeDecision());

        base.StartTurn();
    }

    private IEnumerator StartMakeDecision()
    {
        yield return new WaitForSeconds(1f);

        while (isInTurn)
        {
            MakeDecision(0);
            yield return new WaitForSeconds(Random.Range(1.5f,2.5f));
        }
    }

    public override void MakeDecision(int decision)
    {
        int HitOrStand = Random.Range(0, 2);
        int value = hand.GetHandValue();
        int aceNum = 0;
        bool canSplit = false;

        for (int i = 0; i < hand.Hands[hand.currentHand].Count; i++)
        {
            Card card = hand.Hands[hand.currentHand][i].GetComponent<Card>();
            Card card2 = (i == 0 && hand.Hands[hand.currentHand].Count == 2) ? hand.Hands[hand.currentHand][i + 1].GetComponent<Card>() : null;
            if (card.isAce) aceNum++;
            if (hand.Hands[1] == null && card2 != null && card.value == card2.value) canSplit = true;
        }

        GetDecision(HitOrStand == 0 ? 12 : 11, HitOrStand == 0 ? HitSoft : StandSoft, aceNum, value, canSplit, dealersHand.Hands[0][0].GetComponent<Card>().value);
    }

    private void GetDecision(int v, string[,] strat, int aces, int value, bool canSplit, int dealerValue)
    {
        int yOffset = (aces > 0 ? (canSplit ? v + 8 : v) : (canSplit ? v + 8 : 0));
        int y = 0;
        int x = 0;

        if (yOffset < v)
        {
            if (v == 12)
            {
                switch (value)
                {
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        y = 11;
                        break;
                    case 8:
                        y = 10;
                        break;
                    case 9:
                        y = 9;
                        break;
                    case 10:
                        y = 8;
                        break;
                    case 11:
                        y = 7;
                        break;
                    case 12:
                        y = 6;
                        break;
                    case 13:
                        y = 5;
                        break;
                    case 14:
                        y = 4;
                        break;
                    case 15:
                        y = 3;
                        break;
                    case 16:
                        y = 2;
                        break;
                    case 17:
                        y = 1;
                        break;
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                        y = 0;
                        break;

                }
            }

            else if (v == 11)
            {
                switch (value)
                {
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        y = 10;
                        break;
                    case 8:
                        y = 9;
                        break;
                    case 9:
                        y = 8;
                        break;
                    case 10:
                        y = 7;
                        break;
                    case 11:
                        y = 6;
                        break;
                    case 12:
                        y = 5;
                        break;
                    case 13:
                        y = 4;
                        break;
                    case 14:
                        y = 3;
                        break;
                    case 15:
                        y = 2;
                        break;
                    case 16:
                        y = 1;
                        break;
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                        y = 0;
                        break;

                }
            }
        }
        else if (yOffset < v + 8)
        {
            switch (value)
            {
                case 13:
                    y = 7 + yOffset;
                    break;
                case 14:
                    y = 6 + yOffset;
                    break;
                case 15:
                    y = 5 + yOffset;
                    break;
                case 16:
                    y = 4 + yOffset;
                    break;
                case 17:
                    y = 3 + yOffset;
                    break;
                case 18:
                    y = 2 + yOffset;
                    break;
                case 19:
                    y = 1 + yOffset;
                    break;
                case 20:
                case 21:
                    y = 0 + yOffset;
                    break;

            }
        }
        else if (yOffset >= v + 8)
        {
            switch (value)
            {
                case 4:
                    y = 8 + yOffset;
                    break;
                case 6:
                    y = 7 + yOffset;
                    break;
                case 8:
                    y = 6 + yOffset;
                    break;
                case 10:
                    y = 5 + yOffset;
                    break;
                case 12:
                    y = 4 + yOffset;
                    break;
                case 14:
                    y = 3 + yOffset;
                    break;
                case 16:
                    y = 2 + yOffset;
                    break;
                case 18:
                    y = 1 + yOffset;
                    break;
                case 20:
                    y = 0 + yOffset;
                    break;
            }
        }

        x = dealerValue == 1 ? 9 : dealerValue - 2;

        switch (strat[y,x])
        {
            case "S":
                OnDecisionMade.Invoke(Decision.Stand);
                break;
            case "H":
                OnDecisionMade.Invoke(Decision.Hit);
                break;
            case "U":
                OnDecisionMade.Invoke(Decision.Stand);
                break;
            case "D":
                OnDecisionMade.Invoke(Decision.Hit);
                break;
            case "T":
                OnDecisionMade.Invoke(Decision.Stand);
                break;
        }
    }

    public void MakeBet()
    {
        throw new System.NotImplementedException();
    }

    // "S" 

    private string[,] HitSoft = new string[,]
    {
        {"S","S","S","S","S","S","S","S","S","S"},//12
        {"S","S","S","S","S","S","S","S","S","U"},
        {"S","S","S","S","S","H","H","H","U","U"},
        {"S","S","S","S","S","H","H","H","H","U"},
        {"S","S","S","S","S","H","H","H","H","H"},
        {"S","S","S","S","S","H","H","H","H","H"},
        {"H","H","S","S","S","H","H","H","H","H"},
        {"D","D","D","D","D","D","D","D","D","D"},
        {"D","D","D","D","D","D","D","D","H","H"},
        {"D","D","D","D","D","H","H","H","H","H"},
        {"H","H","H","D","D","H","H","H","H","H"},
        {"H","H","H","H","H","H","H","H","H","H"},

        {"S","S","S","S","S","S","S","S","S","S"},//8
        {"S","S","S","S","D","S","S","S","S","S"},
        {"S","D","D","D","D","S","S","H","H","H"},
        {"D","D","D","D","D","H","H","H","H","H"},
        {"H","H","D","D","D","H","H","H","H","H"},
        {"H","H","D","D","D","H","H","H","H","H"},
        {"H","H","D","D","D","H","H","H","H","H"},
        {"H","H","D","D","D","H","H","H","H","H"},

        {"T","T","T","T","T","T","T","T","T","T"},//10
        {"S","S","S","S","S","S","S","S","S","S"},
        {"T","T","T","T","T","S","T","T","S","T"},
        {"T","T","T","T","T","T","T","T","T","T"},
        {"T","T","T","T","T","T","T","H","U","U"},
        {"T","T","T","T","T","T","H","H","H","H"},
        {"D","D","D","D","D","D","D","D","H","H"},
        {"H","H","T","T","T","H","H","H","H","H"},
        {"T","T","T","T","T","T","T","H","H","H"},
        {"T","T","T","T","T","T","H","H","H","H"}
    };

    private string[,] StandSoft = new string[,]
    {
        {"S","S","S","S","S","S","S","S","S","S"},//11
        {"S","S","S","S","S","H","H","H","U","U"},
        {"S","S","S","S","S","H","H","H","H","H"},
        {"S","S","S","S","S","H","H","H","H","H"},
        {"S","S","S","S","S","H","H","H","H","H"},
        {"H","H","S","S","S","H","H","H","H","H"},
        {"D","D","D","D","D","D","D","D","D","D"},
        {"D","D","D","D","D","D","D","D","H","H"},
        {"D","D","D","D","D","H","H","H","H","H"},
        {"H","H","H","D","D","H","H","H","H","H"},
        {"H","H","H","H","H","H","H","H","H","H"},

        {"S","S","S","S","S","S","S","S","S","S"},//8
        {"S","S","S","S","D","S","S","S","S","S"},
        {"S","D","D","D","D","S","S","H","H","S"},
        {"D","D","D","D","D","H","H","H","H","H"},
        {"H","H","D","D","D","H","H","H","H","H"},
        {"H","H","D","D","D","H","H","H","H","H"},
        {"H","H","D","D","D","H","H","H","H","H"},
        {"H","H","D","D","D","H","H","H","H","H"},

        {"T","T","T","T","T","T","T","T","T","T"},//10
        {"S","S","S","S","S","S","S","S","S","S"},
        {"T","T","T","T","T","S","T","T","S","S"},
        {"T","T","T","T","T","T","T","T","T","T"},
        {"T","T","T","T","T","T","T","H","U","H"},
        {"T","T","T","T","T","T","H","H","H","H"},
        {"D","D","D","D","D","D","D","D","H","H"},
        {"H","H","T","T","T","H","H","H","H","H"},
        {"T","T","T","T","T","T","T","H","H","H"},
        {"T","T","T","T","T","T","H","H","H","H"}
    };
}
