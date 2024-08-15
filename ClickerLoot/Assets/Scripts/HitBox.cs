using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Camera sceneCamera;

    public event Action OnClick;

    private void Start()
    {
        sceneCamera = Camera.main;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !InventorySingleton.Instance.isInvOpen)
        {
            CheckAndHandleCollision();
        }
    }

    private void CheckAndHandleCollision()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            HitBox hitBox = hit.collider.GetComponent<HitBox>();
            if (hitBox != null && hitBox == this)
            {
                OnClick?.Invoke();
            }
        }
    }
}
