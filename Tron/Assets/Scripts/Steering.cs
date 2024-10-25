using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Steering : MonoBehaviour
{
    float maxSteerAngle = 45;
    float currentAngle;
    float targetAngle;

    public delegate void SteeringEvent(float angle);

    public SteeringEvent OnSteer;


    // Update is called once per frame
    void Update()
    {
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, 5 * Time.deltaTime);

        Steer();

        OnSteer.Invoke(currentAngle);
    }

    private float GetAxis(string axis)
    {
        return Input.GetAxis(axis);
    }

    public void Steer()
    {
        targetAngle = GetAxis("Horizontal") * maxSteerAngle;
    }
}
