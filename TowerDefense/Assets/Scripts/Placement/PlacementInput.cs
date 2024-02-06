using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementInput : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Camera sceneCamera;

    public event Action OnClicked, OnExit;

    private Vector3 lastPosition;

    // Tracks the mouse and keyboard input to invoke events
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    /// <summary>
    /// Returns if cursor is over our ui
    /// </summary>
    /// <returns></returns>
    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    /// <summary>
    /// Returns the last position of the mouse over our grid
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
