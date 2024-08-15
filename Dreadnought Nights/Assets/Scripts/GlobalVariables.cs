using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static bool IsInBlackJackPhase { get; set; }
    public static bool RoundIsReady { get; set; }
    public static bool DealerIsFlipping { get; set; }
    public static bool InBettingStage { get; set; }

    public static bool IsGameReadyToStart()
    {
        if (IsInBlackJackPhase) return false;
        if (InBettingStage) return false;
        if (!RoundIsReady) return false;

        return true;
    }
}
