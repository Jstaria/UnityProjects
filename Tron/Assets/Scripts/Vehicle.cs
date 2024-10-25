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

    [Header("Car Var")]
    [SerializeField] private float topCarSpeed;
    private float accelInput;

    [SerializeField] Acceleration acceleration;

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

    [SerializeField] private Steering steering;

    [SerializeField] private GameObject wheelPrefab;

    private List<GameObject> wheels;

    private List<GameObject> groundSpheres;

    private void Start()
    {
        steering.OnSteer += OnSteer;
        acceleration.OnAccel += OnAccel;

        wheels = new List<GameObject>();
        groundSpheres = new List<GameObject>();

        for (int i = 0; i < tires.Count; i++)
        {
            //float distance = Vector3.Distance(tireAttachPoints[i].position, tires[i].transform.position);

            groundSpheres.Add(Instantiate(groundSphere, transform));
            wheels.Add(Instantiate(wheelPrefab, transform));
        }
    }

    private void OnAccel(float accelInput)
    {
        this.accelInput = accelInput;

        Debug.Log(accelInput);
    }

    private void OnSteer(float angle)
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 rotation = wheels[i].transform.rotation.eulerAngles;

            rotation.y = angle;

            wheels[i].transform.rotation = Quaternion.Euler(rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSuspension();
        UpdateFriction();
        UpdateVelocity();
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
        for (int i = 0; i < 2; i++)
        {
            Vector3 accelDir = wheels[i].transform.forward;

            if (accelInput > -1f && accelInput < 1f && accelInput != 0)
            {
                float carSpeed = Vector3.Dot(vehicleBody.transform.forward, vehicleBody.velocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topCarSpeed);

                float availableTorque = .75f * accelInput;

                vehicleBody.AddForce(accelDir * accelInput * topCarSpeed);
            }
        }
    }
}
