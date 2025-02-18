using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public enum SlotsState
{
    Spinning,
    Stopping,
    HasStopped,
    Idle
}

public class Slot : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform stopSpot;
    [SerializeField] private ItemStats stats;

    [Header("Slot Speed Values")]
    [SerializeField] private SlotStats slotStats;

    private Spring slotSpring;

    private List<GameObject> slotItems;

    private float slotSpeed = 1;

    private float randomOffset;

    private bool isSpinning;
    private bool isIdle;
    private bool isInStopSequence;
    private int cardsSince;

    private Coroutine currentSpinStop;
    private Coroutine currentSpinStart;

    public SlotsState currentState = SlotsState.Idle;

    private void Start()
    {
        randomOffset = -2; // Just set for bug reasons

        slotSpring = new Spring(slotStats.angularFrequency, slotStats.dampingRatio, slotStats.defaultSpeed, true);
        slotItems = new List<GameObject>();

        for (int i = slotStats.numItemsSpawned - 1; i >= 0; i--)
        {
            slotItems.Add(Instantiate(stats.GetRandomItem().itemPrefab, spawnPos.position - Vector3.up * slotStats.itemOffset * i + Vector3.up * randomOffset, spawnPos.rotation, spawnPos));
        }
    }

    public void StartSpin(int stopInNumCircle)
    {
        if (currentSpinStop != null)
        {
            StopCoroutine(currentSpinStop);
            currentSpinStop = null;
        }

        //Debug.Log("Spin");
        currentState = SlotsState.Spinning;

        currentSpinStart = StartCoroutine(StartSequence(stopInNumCircle));
    }

    private IEnumerator StartSequence(int stopInNumCircle)
    {
        cardsSince = 0;

        while (cardsSince < stopInNumCircle)
        {
            yield return null;
        }

        //slotSpring.SetValues(slotStats.angularFrequency, slotStats.dampingRatio);
        //slotSpring.RestPosition = -100;

        //yield return new WaitForSeconds(.25f);

        slotSpring.SetValues(slotStats.angularFrequency * 2, slotStats.dampingRatio / 2);
        slotSpring.Position = slotStats.spinSpeed;
        slotSpring.RestPosition = slotStats.spinSpeed;
    }

    public void StopSpin(int stopInNumCards, int numOfStops)
    {
        //Debug.Log("Stop Spinning");
        currentState = SlotsState.Stopping;
        slotSpring.SetValues(slotStats.angularFrequencyStop, slotStats.dampingRatioStop);
        slotSpring.RestPosition = 0;

        currentSpinStop = StartCoroutine(StopSequence(stopInNumCards, numOfStops));
    }

    private IEnumerator StopSequence(int stopInNumCards, int numOfStops)
    {
        isInStopSequence = true;
        cardsSince = 0;

        // Spins until number of cards have spawned;

        while (cardsSince < stopInNumCards)
        {
            yield return null;
        }

        bool hitEnd = false;
        int numBounces = 3;
        float speed = 10;
        float maxSpeed = speed;
        float skipTimer = 0;

        #region old
        /*
        while (currentState == SlotsState.Stopping)
        {
            int cardIndex = slotItems.Count / 2 - 1;

            for (int i = 0; i < slotItems.Count; i++)
            {
                GameObject item = slotItems[i];

                HasBounced(item, i, cardIndex, out hitEnd, stopSpot.position.y + slotStats.itemOffset * (i - cardIndex));

                if (hitEnd && speed > .1f && skipTimer <= 0)
                {
                    hadFirstBounce = true;

                    slotSpeed = -speed;
                    speed -= (1 / (float)numBounces * maxSpeed);

                    skipTimer = .1f;

                    break;
                }
            }

            skipTimer -= Time.deltaTime;

            if (hadFirstBounce)
                slotSpeed += 10 * Time.deltaTime;

            if (speed <= .1f)
                currentState = SlotsState.HasStopped;
        

            yield return null;
        }
        */
        #endregion


        for (int i = 0; i < numOfStops; i++)
        {
            bool moveOn = false;

            int cardIndex = slotStats.numItemsSpawned - 3;

            cardsSince = 0;

            while (moveOn == false || cardsSince == 0)
            {
                for (int j = 0; j < slotItems.Count; j++)
                {
                    GameObject item = slotItems[j];

                    if (!moveOn)
                        HasBounced(item, j, cardIndex, out hitEnd, stopSpot.position.y + slotStats.itemOffset * (j - cardIndex));

                    if (hitEnd && skipTimer <= 0)
                    {
                        GameObject wonItem = slotItems[cardIndex];

                        LootCollector.Instance.AddItem(Instantiate(wonItem, wonItem.transform.position, wonItem.transform.rotation, LootCollector.Instance.transform), -maxSpeed);
                        wonItem.SetActive(false);

                        moveOn = true;
                        hitEnd = false;

                        slotSpeed = -maxSpeed;

                        skipTimer = .1f;

                        break;
                    }
                }

                skipTimer -= Time.deltaTime;

                if (moveOn)
                {
                    slotSpeed += (30 + MathF.Pow(i, 2f)) * Time.deltaTime ;
                }

                // Debug.Log(string.Format("{0} Speed:" + slotSpeed, name));
                // Debug.Log(string.Format("{0} moveOn:" + moveOn, name));

                yield return null;
            }
        }

        //slotSpring.SetValues(slotStats.angularFrequency, slotStats.angularFrequency); ;
        slotSpring.Position = slotStats.defaultSpeed;
        slotSpring.RestPosition = slotStats.defaultSpeed;

        if (currentState != SlotsState.Spinning)
        {
            currentState = SlotsState.Idle;
        }

    }

    private void HasBounced(GameObject item, int i, int cardIndex, out bool hitEnd, float positionY)
    {
        SetItemYPos(i, Mathf.Clamp(item.transform.position.y, positionY, item.transform.position.y));

        hitEnd = MathF.Round(item.transform.position.y, 2) == MathF.Round(positionY, 2);
    }

    // Update is called once per frame
    void Update()
    {
        slotSpring.Update();

        if (currentState == SlotsState.Spinning || currentState == SlotsState.Idle)
            slotSpeed = slotSpring.Position;

        MoveItems(slotSpeed, true);

        if (slotItems.Count < slotStats.numItemsSpawned)
        {
            if (!isSpinning)
                cardsSince++;

            slotItems.Add(Instantiate(stats.GetRandomItem().itemPrefab, slotItems[slotItems.Count - 1].transform.position + Vector3.up * slotStats.itemOffset, spawnPos.rotation, spawnPos));
        }
    }

    private void SetItemYPos(int index, float positionY)
    {
        GameObject item = slotItems[index];

        Vector3 position = item.transform.position;

        position.y = positionY;

        item.transform.position = position;
    }

    private void MoveItems(float slotSpeed, bool useDeltaTime)
    {
        for (int i = 0; i < slotItems.Count; i++)
        {
            GameObject item = slotItems[i];

            Vector3 position = item.transform.position;
            position.y -= slotSpeed * (useDeltaTime ? Time.deltaTime : 1);
            item.transform.position = position;

            if (Vector3.Distance(item.transform.position, spawnPos.position) > slotStats.itemOffset * (slotStats.numItemsSpawned - 1) - randomOffset)
            {
                Destroy(item.gameObject);
                slotItems.RemoveAt(i);
                i--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(slotItems[slotStats.numItemsSpawned - 3].transform.position, 3);
    }
}
