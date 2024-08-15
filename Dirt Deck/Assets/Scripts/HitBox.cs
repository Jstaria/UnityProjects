using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] public Camera sceneCamera;

    public event Action OnClick;

    private Rect hbRect;
    public Vector2 size;

    private void Start()
    {

    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckAndHandleCollision();
        }

        hbRect = new Rect(transform.position - (Vector3)(size * .5f), size);
    }

    public bool IsMouseOver()
    {
        return hbRect.Contains(sceneCamera.ScreenToWorldPoint(Input.mousePosition));
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(new Vector3(hbRect.center.x, hbRect.center.y, 0.01f), new Vector3(hbRect.size.x, hbRect.size.y, 0.01f));
    }
}
