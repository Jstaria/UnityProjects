using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayWithinBounds : MonoBehaviour
{
    public Transform bounds;
    public Transform hitBox;

    public void StayInBounds()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, bounds.position.x - bounds.lossyScale.x / 2, bounds.position.x + bounds.lossyScale.x / 2);
        position.y = Mathf.Clamp(position.y, bounds.position.y - bounds.lossyScale.y / 2, bounds.position.y + bounds.lossyScale.y / 2);

        this.transform.position = position;
    }
}
