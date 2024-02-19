using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraArm;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        cameraArm.position = Vector3.Lerp(cameraArm.position, playerPosition.position + offset, .5f);
    }
}
