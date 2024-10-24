using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bike : MonoBehaviour
{
    //[SerializeField] private GameObject wheelPrefab;

    [SerializeField] private float wheelGrip;
    [SerializeField] private float wheelSpeed;

    [SerializeField] private Wheel frontWheel; // Turn wheel
    [SerializeField] private Wheel backWheel; // Drive wheel

    [SerializeField] private LayerMask ground;
    [SerializeField] private float rayDistance;

    [SerializeField] private AnimationCurve torqueCurve;

    // Start is called before the first frame update
    void Start()
    {
        frontWheel.SetMaxSpeed(wheelSpeed);
        backWheel.SetMaxSpeed(wheelSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSteer(float angle)
    {
        frontWheel.TurnTo(angle);
    }

    public void OnAcceleration(float accel)
    {
        if (CheckGroundCollision(frontWheel.transform, ground, rayDistance))
        {
            //frontWheel.ApplyForce(accel, torqueCurve.Evaluate(accel));
            frontWheel.ApplyForce(accel, .5f);
        }

        if (CheckGroundCollision(backWheel.transform, ground, rayDistance))
        {
            //backWheel.ApplyForce(accel, torqueCurve.Evaluate(accel));
            backWheel.ApplyForce(accel, .5f);
        }
    }

    private bool CheckGroundCollision(Transform wheel, LayerMask ground, float rayDistance)
    {
        Ray ray = new Ray(wheel.position, -wheel.up);

        return Physics.Raycast(ray, rayDistance, ground);
    }
}
