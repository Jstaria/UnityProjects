using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

enum SlotState
{
    Starting,
    Spinning,
    Stopped
}

// TODO: Must add a way to both set items into slots by txt file maybe, and grab won items from wheel
public class SlotMachine : MonoBehaviour
{
    [Header("Slot Components")]
    [SerializeField] private List<GameObject> SlotWheels;
    [SerializeField] private ItemPicker itemPicker;

    [Header("Slot Wheel Variables")]
    [SerializeField] private float maxRpm = 10;
    [SerializeField] private float timeInbetweenStop = 1; 

    [Header("Slot Wheel Won Slot")]
    [SerializeField] private SlotCollider[] wonItems;

    private SlotState currentState = SlotState.Starting;
    private float currentRpm;
    private float lastStopTime = 0;

    // Starting up variables
    public int currentWheel = 0;

    //private SlotState previousState = SlotState.Stopped;

    private void Start()
    {
        currentRpm = Random.Range(maxRpm - 1, maxRpm + 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case SlotState.Starting:

                wonItems = new SlotCollider[SlotWheels.Count];

                SlotWheel slotWheel = SlotWheels[currentWheel].GetComponent<SlotWheel>();

                slotWheel.winCollider = null;
                slotWheel.Stopped = false;
                slotWheel.StartSpin(currentRpm);
                
                if (slotWheel.Rpm >= currentRpm)
                {
                    currentRpm = Random.Range(maxRpm - 1, maxRpm + 1);
                    currentWheel++;
                }
                
                if (currentWheel >= SlotWheels.Count)
                {
                    currentWheel = 0;
                    currentState = SlotState.Spinning;
                }

                break;

            case SlotState.Spinning:

                for (int i = 0; i < SlotWheels.Count; i++)
                {
                    wonItems[i] = SlotWheels[i].GetComponent<SlotWheel>().PickItem();
                }

                currentState = SlotState.Stopped;

                break;

            case SlotState.Stopped:

                if (currentWheel >= SlotWheels.Count) { break; }

                slotWheel = SlotWheels[currentWheel].GetComponent<SlotWheel>();

                wonItems[currentWheel] = slotWheel.winCollider;

                bool canStop = Time.time - lastStopTime >= timeInbetweenStop;

                if (CheckCollision(wonItems[currentWheel], itemPicker.Colliders[currentWheel]) && canStop)
                {
                    lastStopTime = Time.time;
                    slotWheel.StopSpin();
                   
                    slotWheel.winCollider.won = true;
                    wonItems[currentWheel].won = true;
                    currentWheel++;
                }

                break;
        }

        GetWonItems();
    }

    private void GetWonItems()
    {
        for (int i = 0; i < SlotWheels.Count; i++)
        {
            SlotWheel slotWheel = SlotWheels[i].GetComponent<SlotWheel>();

            for (int j = 0; j < slotWheel.Colliders.Count; j++)
            {
                bool inCollision = false;

                if (CheckCollision(slotWheel.Colliders[j], itemPicker.Colliders[i]))
                {
                    inCollision = true;
                }

                slotWheel.Colliders[j].inCollision = inCollision;
            }
        }
    }

    private bool CheckCollision(SlotCollider c1, SlotCollider c2)
    {
        return (c1.transform.position - c2.transform.position).magnitude < c1.radius + c2.radius;
    }
}
