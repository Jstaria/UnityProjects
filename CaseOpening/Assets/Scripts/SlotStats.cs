using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotStatsEnum
{
    AngularFrequency,
    AngularFrequencyStop,
    DampingRatio,
    DampingRatioStop,
    DefaultSpeed,
    SpinSpeed,
    NumberOfItems,
    ItemOffset
}

[CreateAssetMenu]
public class SlotStats : ScriptableObject
{
    public float angularFrequency;
    public float dampingRatio;
    public float angularFrequencyStop;
    public float dampingRatioStop;
    public float defaultSpeed = 1;
    public float spinSpeed = 20;
    public int numItemsSpawned = 5;
    public float itemOffset = 3.1f;

    public void SetVariable(SlotStatsEnum stat, float value)
    {
        switch (stat)
        {
            case SlotStatsEnum.AngularFrequency:
                angularFrequency = value;
                break;

            case SlotStatsEnum.AngularFrequencyStop:
                angularFrequencyStop = value;
                break;

            case SlotStatsEnum.DampingRatio:
                dampingRatio = value;
                break;

            case SlotStatsEnum.DampingRatioStop:
                dampingRatioStop = value;
                break;

            case SlotStatsEnum.DefaultSpeed:
                defaultSpeed = Mathf.Abs(value);
                break;

            case SlotStatsEnum.SpinSpeed:
                spinSpeed = Mathf.Abs(value);
                break;

            case SlotStatsEnum.NumberOfItems:
                numItemsSpawned = Mathf.Abs((int)value);
                break;

            case SlotStatsEnum.ItemOffset:
                itemOffset = value;
                break;
        }
    }
}
