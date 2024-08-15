using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerAI : IPlayer
{
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
            while (!GlobalVariables.DealerIsFlipping)
            {
                yield return new WaitForEndOfFrame();
            }

            MakeDecision(0);
            yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
        }
    }

    public override void MakeDecision(int decision)
    {
        int value = hand.GetHandValue();

        if (value < 17)
        {
            OnDecisionMade.Invoke(Decision.Hit);
        }
        else
        {
            OnDecisionMade.Invoke(Decision.Stand);
        }

    }
}
