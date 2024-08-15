using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public enum LookDirection
{
    Left,
    Right,
    Up,
    Down,
    Behind,
    Forward,
}

public class MouseCameraController : MonoBehaviour
{
    public FocusDirections directionsObject;

    // Use scriptable objects to set this
    private Dictionary<LookDirection, FocusOn> directions;

    private LookDirection[] horizontalDirections =
        new LookDirection[]
        {
            LookDirection.Left,
            LookDirection.Forward,
            LookDirection.Right,
            LookDirection.Behind
        };

    private LookDirection[] verticalDirections =
        new LookDirection[]
        {
            LookDirection.Up,
            LookDirection.Forward,
            LookDirection.Down,
        };

    public float MaxOffset = 30;
    public float zoomOffset = 20;

    private float currentOffset;

    public CameraController cameraCon;

    public LookDirection currentFocusDirection = LookDirection.Forward;
    public LookDirection prevFocusDirection;

    // -1 to 1
    public float mousePositionXValue;
    public float mousePositionYValue;

    private float FOVoffset;

    private void Start()
    {
        SetDirections();
    }

    private void SetDirections()
    {
        directions = new Dictionary<LookDirection, FocusOn>();

        for (int i = 0; i < directionsObject.focuses.Count; i++)
        {
            directions.Add(directionsObject.focuses[i].direction, directionsObject.focuses[i]);
        }
    }

    private void Update()
    {
        float width = Screen.width;
        float height = Screen.height;

        Vector2 mousePos = Input.mousePosition;

        mousePositionXValue = Mathf.Clamp((mousePos.x - (width / 2)) / width * 2, -1, 1);
        mousePositionYValue = Mathf.Clamp((mousePos.y - (height / 2)) / height * 2, -1, 1);

        cameraCon.SetFocusOffset(new Vector3(-mousePositionYValue * currentOffset, mousePositionXValue * currentOffset));

        if (Input.GetMouseButtonDown(0) && cameraCon.Contains(Input.mousePosition)) ChangeFocus();
        ZoomCheck();
    }

    private void ZoomCheck()
    {
        FOVoffset = 0;
        currentOffset = MaxOffset;

        if (Input.GetMouseButton(1))
        {
            FOVoffset = -40;
            currentOffset = MaxOffset + zoomOffset;
        }

        cameraCon.SetFOVOffset(FOVoffset);
    }

    private void ChangeFocus()
    {
        if (mousePositionXValue < -.75f)
        {
            if (currentFocusDirection == LookDirection.Up || currentFocusDirection == LookDirection.Down) return;

            int spot = 0;

            for (spot = 0; spot < horizontalDirections.Length; spot++)
            {
                if (horizontalDirections[spot] == currentFocusDirection) break;
            }

            int direction = (spot - 1) % (horizontalDirections.Length);

            if (direction < 0) direction += horizontalDirections.Length;

            prevFocusDirection = currentFocusDirection;
            currentFocusDirection = horizontalDirections[direction];
        }

        if (mousePositionXValue > .75f)
        {
            if (currentFocusDirection == LookDirection.Up || currentFocusDirection == LookDirection.Down) return;

            int spot = 0;

            for (spot = 0; spot < horizontalDirections.Length; spot++)
            {
                if (horizontalDirections[spot] == currentFocusDirection) break;
            }

            int direction = (spot + 1) % (horizontalDirections.Length);

            prevFocusDirection = currentFocusDirection;
            currentFocusDirection = horizontalDirections[direction];
        }

        if (!directions.ContainsKey(currentFocusDirection))
        {
            currentFocusDirection = prevFocusDirection;
            return;
        } 
        cameraCon.SetFocus(directions[currentFocusDirection]);
    }
}
