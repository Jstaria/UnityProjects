using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private GameObject slotCollider;
    [SerializeField] private int wheelAmount = 3;

    private float turnAmount = -(Mathf.PI * 2 / 11) * 2.75f;
    private float radius = 1f;
    private List<SlotCollider> colliders = new List<SlotCollider>();

    public List<SlotCollider> Colliders { get { return colliders; } }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = -1; i < wheelAmount - 1; i++)
        {
            int y = i;
            float turnAmount = this.turnAmount;

            if (transform.parent.rotation.y < 0) 
            { 
                turnAmount = -this.turnAmount;
                y = -y;
            }

            colliders.Add(Instantiate(slotCollider, new Vector3(transform.position.x + (y * .5f), radius * Mathf.Cos(turnAmount) + transform.position.y, radius * Mathf.Sin(turnAmount) + transform.position.z), Quaternion.identity, transform).GetComponent<SlotCollider>());
            colliders[colliders.Count - 1].radius = .1f;
            colliders[colliders.Count - 1].TurnAmount = turnAmount;
            colliders[colliders.Count - 1].ItemID = 0;
        }
    }
}
