using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private List<GameObject> tires;
    [SerializeField] private List<Transform> tireAttachPoints;

    private List<Spring> springs;

    [SerializeField] private AnimationCurve frictionCurve;

    [SerializeField] private float angularVelocity;
    [SerializeField] private float dampingRatio;

    [SerializeField] private float wheelRestDistance = 1;

    [SerializeField] private Rigidbody vehicleBody;
    [SerializeField] private LayerMask ground;

    private void Start()
    {
        springs = new List<Spring>();

        for (int i = 0; i < tires.Count; i++)
        {
            float distance = Vector3.Distance(tireAttachPoints[i].position, tires[i].transform.position);

            springs.Add(new Spring(angularVelocity, dampingRatio, distance));
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Spring spr in springs)
        {
            spr.Update();
        }

        UpdateSuspension();
    }

    private void UpdateSuspension()
    {
        for (int i = 0; i < tires.Count; i++)
        { 
            Ray ray = new Ray(tires[i].transform.position, -tires[i].transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, wheelRestDistance, ground))
            {
                Vector3 springDir = tires[i].transform.up;
                Vector3 tireWorldVel = vehicleBody.GetPointVelocity(tires[i].transform.position);

                float offset = wheelRestDistance - hit.distance;

                float velocity = Vector3.Dot(springDir, tireWorldVel);

                float force = (offset * angularVelocity) - (velocity * dampingRatio);

                vehicleBody.AddForceAtPosition(springDir * force, tires[i].transform.position);
            }
        }
    }

    private void UpdateFriction()
    {

    }

    private void UpdateVelocity()
    {

    }
}
