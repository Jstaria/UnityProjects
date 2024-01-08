using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCollider : MonoBehaviour
{
    public float turnAmount;

    public bool inCollision { get; set; }
    public float TurnAmount { get { return turnAmount; } set { turnAmount = value; } }
    public float radius { get; set; }
    public bool won { get; set; }
    public int ItemID { get; set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (inCollision)
        {
            Gizmos.color = Color.green;
        }
        if (won)
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
