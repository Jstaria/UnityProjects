using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [SerializeField] private Transform cameraLockPoint;
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 swayAllowance = new Vector2(5,5);

    [SerializeField] private float lerpSpeed = 5;

    private void Update()
    {
        Vector3 offset = Global.Instance.playerDirection + Global.Instance.playerPosition;

        offset.x = Mathf.Clamp(offset.x, -swayAllowance.x, swayAllowance.x);
        offset.y = Mathf.Clamp(offset.y, -swayAllowance.y, swayAllowance.y);
        offset.z = cam.transform.position.z; 

        cam.transform.position = Vector3.Lerp(cameraLockPoint.position + offset, cam.transform.position, Mathf.Pow(.5f, Time.deltaTime * lerpSpeed));
    }
}
