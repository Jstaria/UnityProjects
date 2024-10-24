using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Rigidbody vehicleBody;
    
    public float MaxSpeed {  get; private set; }
    public float TireGrip { get; private set; }

    public void TurnTo(float angle)
    {
        Vector3 rotation = transform.localRotation.eulerAngles;

        rotation.y = angle;

        transform.localRotation = Quaternion.Euler(rotation);
    }

    public void ApplyForce(float accelSpeed, float torque)
    {
        Vector3 direction = transform.forward;

        vehicleBody.AddForceAtPosition(direction * torque * accelSpeed, transform.position);
    }

    public void SetMaxSpeed(float speed)
    {
        MaxSpeed = speed;
    }
    public void SetTireGrip(float grip)
    {
        TireGrip = grip;
    }
}
