using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private List<GameObject> tires;
    [SerializeField] private List<Transform> tireAttachPoints;

    [SerializeField] private AnimationCurve frictionCurve;

    [Header("Suspension Var")]

    [SerializeField] private float angularVelocity;
    [SerializeField] private float dampingRatio;

    [SerializeField] private float wheelRestDistance = 1;

    [SerializeField] private Rigidbody vehicleBody;
    [SerializeField] private LayerMask ground;

    [SerializeField] private GameObject groundSphere;

    [Header("Wheel Var")]
    [SerializeField] private float tireGrip;
    [SerializeField] private float tireMass;

    [SerializeField] private GameObject wheelPrefab;

    private List<GameObject> wheels;

    private List<GameObject> groundSpheres;

    private void Start()
    {
        wheels = new List<GameObject>();
        groundSpheres = new List<GameObject>();

        for (int i = 0; i < tires.Count; i++)
        {
            //float distance = Vector3.Distance(tireAttachPoints[i].position, tires[i].transform.position);

            groundSpheres.Add(Instantiate(groundSphere, transform));
            wheels.Add(Instantiate(wheelPrefab, transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSuspension();
        UpdateFriction();
    }

    private void UpdateSuspension()
    {
        for (int i = 0; i < tires.Count; i++)
        { 
            Ray ray = new Ray(tires[i].transform.position, -tires[i].transform.up);
            RaycastHit hit;

            float distance = wheelRestDistance;

            if (Physics.Raycast(ray, out hit, wheelRestDistance, ground))
            {
                Vector3 springDir = tires[i].transform.up;
                Vector3 tireWorldVel = vehicleBody.GetPointVelocity(tires[i].transform.position);

                float offset = wheelRestDistance - hit.distance;

                float velocity = Vector3.Dot(springDir, tireWorldVel);

                float force = (offset * angularVelocity) - (velocity * dampingRatio);

                vehicleBody.AddForceAtPosition(springDir * force, tires[i].transform.position);

                distance = hit.distance;
            }

            groundSpheres[i].transform.position = tires[i].transform.position - tires[i].transform.up * distance;
            wheels[i].transform.position = tires[i].transform.position - tires[i].transform.up * distance + new Vector3(0, wheels[i].transform.localScale.y * 2);

            //if (wheels[i].transform.position.y < 0.03 + wheels[i].transform.localScale.y * 2) wheels[i].transform.position.Set(wheels[i].transform.position.x, wheels[i].transform.localScale.y * 2, wheels[i].transform.position.z);
        }
    }

    private void UpdateFriction()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            Ray ray = new Ray(tires[i].transform.position, -tires[i].transform.up);
            RaycastHit hit;

            float distance = wheelRestDistance;

            if (Physics.Raycast(ray, out hit, wheelRestDistance, ground))
            {
                Vector3 steeringDir = wheels[i].transform.right;
                Vector3 tireWorldVel = vehicleBody.GetPointVelocity(wheels[i].transform.position);

                float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

                float desiredVelChange = -steeringVel * tireGrip;

                float desiredAccel = desiredVelChange / Time.deltaTime;

                vehicleBody.AddForceAtPosition(steeringDir * tireMass * desiredAccel, wheels[i].transform.position);
            }
        }
    }

    private void UpdateVelocity()
    {

    }
}
