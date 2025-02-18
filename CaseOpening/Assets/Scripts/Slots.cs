using Mono.CSharp;
using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour
{
    [SerializeField] private SlotStats stats;
    [SerializeField] private List<Slot> slots;

    private Coroutine startSequence;
    private Coroutine stopSequence;

    private bool isSpinning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    public void StartSpin(int numOfStops)
    {
        if (isSpinning)
            return;

        EventBus.Instance.NotifyListeners("SlotsButtonSwitchOff");
        isSpinning = true;

        if (startSequence != null)
        {
            StopCoroutine(startSequence);
            startSequence = null;
        }

        startSequence = StartCoroutine(StartSequence());
        stopSequence = StartCoroutine(StopSequence(numOfStops));
    }

    private IEnumerator StartSequence()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].StartSpin(0);

            yield return new WaitForSeconds(.05f);
        }
    }

    [Command]
    public void StopSpin(int numOfStops)
    {
        for (int i = 0; i < slots.Count; i++) 
        {
            slots[i].StopSpin(i * 4 + 5, numOfStops);
        }
    }

    private IEnumerator StopSequence(int numOfStops)
    {
        bool allStopped = false;

        yield return new WaitForSeconds(1);

        StopSpin(numOfStops);

        while (!allStopped)
        {
            int count = 0;

            for(int i = 0;i < slots.Count;i++)
            {
                if (slots[i].currentState == SlotsState.HasStopped || slots[i].currentState == SlotsState.Idle)
                    count++;
            }

            allStopped = count == slots.Count;

            yield return null;
        }

        //yield return new WaitForSeconds(1);

        yield return StartCoroutine(LootCollector.Instance.ItemStackSuck());

        LootCollector.Instance.ClearWonItems();
        EventBus.Instance.NotifyListeners("SlotsButtonSwitchOn");
        isSpinning = false;
    }

    [Command]
    public void SetVariable(SlotStatsEnum stat, float value)
    {
        stats.SetVariable(stat, value);
    }
}
