using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        Global.Instance.playerPosition = transform.position;
        Global.Instance.playerVelocity = GetComponent<MovementController>().Direction;
    }
}
