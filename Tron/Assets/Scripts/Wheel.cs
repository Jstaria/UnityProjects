using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Rigidbody vehicleBody;

    private Vector3 currentForce;
    private Vector3 currentGrip;


    public float MaxSpeed { get; private set; }
    public float TireGrip { get; private set; }

    private void Update()
    {
        ApplyFriction();
    }

    public void TurnTo(float angle)
    {
        Vector3 rotation = transform.localRotation.eulerAngles;

        rotation.y = angle;

        transform.localRotation = Quaternion.Euler(rotation);
    }

    public void ApplyAccel(float accelSpeed, float torque)
    {
        Vector3 direction = transform.right;

        currentForce = direction * torque * accelSpeed * MaxSpeed;

        //Debug.Log(currentForce);

        vehicleBody.AddForceAtPosition(currentForce, transform.position);
    }

    public void ApplyFriction()
    {
        Vector3 steeringDir = transform.up;
        Vector3 tireWorldVel = vehicleBody.GetPointVelocity(transform.position);

        float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

        float desiredVelChange = -steeringVel * TireGrip;

        float desiredAccel = desiredVelChange / Time.deltaTime;

        currentGrip = steeringDir * 5 * desiredAccel;

        vehicleBody.AddForceAtPosition(currentGrip, transform.position);
    }

    public void SetMaxSpeed(float speed)
    {
        MaxSpeed = speed;
    }
    public void SetTireGrip(float grip)
    {
        TireGrip = grip;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + currentForce);

        Vector3 tireWorldVel = vehicleBody.GetPointVelocity(transform.position);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + currentGrip);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
    }
}
