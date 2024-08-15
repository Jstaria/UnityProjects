using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public abstract class IPlayer : MonoBehaviour
{
    public UnityEvent OnStartTurn;
    public UnityEvent OnEndTurn;

    public UnityEvent<Decision> OnDecisionMade;

    public bool isInTurn;

    public Hand hand;
    public ChipBank chipBank;

    public abstract void MakeDecision(int decision);

    public bool IsInTurn()
    {
        return isInTurn;
    }

    public void StartTurn()
    {
        isInTurn = true;
        OnStartTurn.Invoke();
    }

    public void EndTurn()
    {
        isInTurn = false;
        OnEndTurn.Invoke();
    }
}

public interface IBet
{
    public abstract void MakeBet();
}
