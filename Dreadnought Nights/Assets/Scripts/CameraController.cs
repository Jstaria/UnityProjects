using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public FocusDirections directions;
    private FocusOn targetFocus;

    public Camera camera;
    public float lerpSpeed = 5;

    private Vector3 offset = Vector3.zero;
    private float FOVOffset;

    private void Start()
    {
        foreach (var direction in directions.focuses)
        {
            if (direction.direction == LookDirection.Forward)
            {
                targetFocus = direction;
                break;
            }
        }

        transform.position = targetFocus.position;
        transform.rotation = Quaternion.Euler(targetFocus.rotation);
        camera.fieldOfView = targetFocus.FOV;
    }

    private void Update()
    {
        float lerp = Mathf.Pow(.5f, lerpSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(targetFocus.position, transform.position, lerp);
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(targetFocus.rotation + offset), transform.rotation, lerp);
        camera.fieldOfView = Mathf.Lerp(targetFocus.FOV + FOVOffset, camera.fieldOfView, lerp);
    }

    internal void SetFocus(FocusOn currentFocus)
    {
        targetFocus = currentFocus;
    }

    public void SetFocusOffset(Vector3 offset)
    {
        this.offset = offset; 
    }

    internal bool Contains(Vector3 position)
    {
        return 
            position.x >= 0 && position.x <= Screen.width &&
            position.y >= 0 && position.y <= Screen.height;
    }

    internal void SetFOVOffset(float fOVoffset)
    {
        FOVOffset = fOVoffset;
    }
}
