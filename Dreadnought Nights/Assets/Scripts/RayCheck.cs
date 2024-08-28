using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheck : MonoBehaviour
{
    public LayerMask mask;
    public Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public bool CheckMouseOver()
    {
        bool wasHit = false;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            RayCheck rayCheck = hit.collider.GetComponent<RayCheck>();
            if (rayCheck != null && rayCheck == this)
            {
                wasHit = true;
            }
        }

        return wasHit;
    }
}
