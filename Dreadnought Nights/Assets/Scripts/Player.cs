using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : IPlayer, IBet
{
    public void MakeBet()
    {
        
    }

    public override void MakeDecision(int decision)
    {
        OnDecisionMade.Invoke((Decision)decision);
    }    
}
