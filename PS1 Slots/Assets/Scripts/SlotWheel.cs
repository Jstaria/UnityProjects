using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Properties;
using UnityEngine;
using UnityEngine.XR;

public class SlotWheel : MonoBehaviour
{
    [SerializeField] private int numberOfSides = 11;
    [SerializeField] private float colliderRadius = .2f;
    [SerializeField] private GameObject slotCollider;

    public float rpm = Mathf.Epsilon;
    private float radius = 1;
    private List<SlotCollider> colliders = new List<SlotCollider>();
    private List<bool> collisions = new List<bool>();
    private float turnAmount = 0;
    private float stoppedTurnAmount;

    public float winDiff;

    public SlotCollider winCollider { get; set; }
    public List<SlotCollider> Colliders { get { return colliders; } }
    public float Rpm { get { return rpm; } set { rpm = value; } }
    public bool Stopped { get; set; }

    private void Start()
    {
        Quaternion currentRotation = transform.rotation;

        transform.rotation = Quaternion.Euler(0,0,0);

        float rotationPI = (Mathf.PI * 2) / numberOfSides;

        for (int i = 0; i < numberOfSides; i++)
        {
            colliders.Add(Instantiate(slotCollider, new Vector3(transform.position.x, radius * Mathf.Cos(-rotationPI * (i + .75f)) + transform.position.y, radius * Mathf.Sin(-rotationPI * (i + .75f)) + transform.position.z), Quaternion.identity, transform).GetComponent<SlotCollider>());
            colliders[colliders.Count - 1].radius = colliderRadius;
            colliders[colliders.Count - 1].TurnAmount = rotationPI * (i + .75f) * Mathf.Rad2Deg - 78;
            colliders[colliders.Count - 1].ItemID = i;
        }

        transform.rotation = currentRotation;
    }

    private void FixedUpdate()
    {
        turnAmount -= rpm;
        turnAmount %= 360;

        transform.localRotation = Quaternion.Euler(turnAmount, transform.localRotation.y, transform.localRotation.z);
    }

    public void StartSpin(float rpm)
    {
        if (this.rpm < rpm)
        {
            if (this.rpm < .01f)
            {
                this.rpm += 1;
            }

            this.rpm *= 1.05f;
        } 
    }

    public SlotCollider PickItem()
    {
        // Pick random collider to stop on
        winCollider = colliders[Random.Range(0, colliders.Count)];

        return winCollider;
    }

    public void StopSpin()
    {
        // Get that collider's rotation amount
        turnAmount = winCollider.turnAmount;

        // Set the final goal rotation for the wheel

        rpm = 0;
        Stopped = true;    

        if (rpm < .1f) { rpm = Mathf.Epsilon; }
    }

    public void SpinToItem(float turnAmount)
    {
        stoppedTurnAmount = turnAmount;
        this.turnAmount %= 360;
    }
}
