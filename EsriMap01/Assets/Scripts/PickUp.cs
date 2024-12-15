using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public LayerMask pickup;

    public float distance;

    public Camera cam;

    public GameObject grabbedObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedObj != null)
        {
            Vector3 position = cam.transform.position + cam.transform.forward * distance;
            position.y = cam.transform.position.y;

            grabbedObj.transform.position = position;
        }

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            if (grabbedObj != null)
            {
                grabbedObj.GetComponent<Rigidbody>().useGravity = true;
                grabbedObj = null;
            }
            else
            {
                Ray ray = new Ray(cam.transform.position, cam.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distance, pickup))
                {
                    grabbedObj = hit.collider.gameObject;
                    grabbedObj.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * distance);
    }
}
