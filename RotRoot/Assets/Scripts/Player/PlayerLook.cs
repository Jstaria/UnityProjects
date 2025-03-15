using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerStats playerStats;

    public Vector2 LastDirection {  get; private set; }

    private Quaternion q_Rot;

    public void LookUpdate(Vector2 LookDirection)
    {
        LastDirection = LookDirection;

        float angle = Mathf.Atan2(LookDirection.y, LookDirection.x);

        float lerp = Mathf.Pow(.5f, playerStats.sensitivity.x * Time.deltaTime);

        q_Rot = Quaternion.Slerp(Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg)), q_Rot, lerp);

        playerTransform.transform.rotation = q_Rot;
    }
}

