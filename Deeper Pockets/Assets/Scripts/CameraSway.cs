using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [SerializeField] private Transform cameraLockPoint;
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 swayAllowance = new Vector2(7,7);
    [SerializeField] private float mouseInfluence = 2;

    [SerializeField] private float lerpSpeed = 5;

    private Vector3 offset;

    private void FixedUpdate()
    {
        offset = Vector3.Lerp(offset, Global.Instance.playerVelocity + Global.Instance.playerPosition + MouseOffset(), lerpSpeed);

        //offset.x = Mathf.Clamp(offset.x, -swayAllowance.x, swayAllowance.x);
        //offset.y = Mathf.Clamp(offset.y, -swayAllowance.y, swayAllowance.y);
        offset.z = cam.transform.position.z;

        cam.transform.position = cameraLockPoint.position + offset;
    }

    private Vector2 MouseOffset()
    {
        if (mouseInfluence == 0) return Vector2.zero;

        Vector2 mousePos = Input.mousePosition;
        
        float mousePositionXValue, mousePositionYValue;
        float width = Screen.width;
        float height = Screen.height;

        mousePositionXValue = Mathf.Clamp((mousePos.x - cam.WorldToScreenPoint(Global.Instance.playerPosition).x) / width * 2, -1, 1);
        mousePositionYValue = Mathf.Clamp((mousePos.y - cam.WorldToScreenPoint(Global.Instance.playerPosition).y) / height * 2, -1, 1);

        return new Vector2(mousePositionXValue * mouseInfluence,mousePositionYValue * mouseInfluence);
    }
}
