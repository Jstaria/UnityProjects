using QFSW.QC;
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

    [SerializeField] private Steering steer;
    [SerializeField] private Acceleration accel;

    [SerializeField] private Rigidbody vehicleBody;

    // Start is called before the first frame update
    void Start()
    {
        frontWheel.SetMaxSpeed(wheelSpeed);
        frontWheel.SetTireGrip(wheelGrip);
        backWheel.SetMaxSpeed(wheelSpeed);
        backWheel.SetTireGrip(wheelGrip);

        steer.OnSteer += OnSteer;
        accel.OnAccel += OnAcceleration;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = vehicleBody.transform.rotation.eulerAngles;
        rotation.x = 0;

        vehicleBody.transform.rotation = Quaternion.Euler(rotation);
    }

    public void OnSteer(float angle)
    {
        frontWheel.TurnTo(angle);
    }

    public void OnAcceleration(float accel)
    {
        //Debug.Log(accel);

        if (CheckGroundCollision(backWheel.transform, ground, rayDistance))
        {
            //backWheel.ApplyForce(accel, torqueCurve.Evaluate(accel));
            backWheel.ApplyAccel(accel, .5f);
            frontWheel.ApplyAccel(accel, .75f);
        }
    }

    private bool CheckGroundCollision(Transform wheel, LayerMask ground, float rayDistance)
    {
        Ray ray = new Ray(wheel.position, Vector3.down);

        RaycastHit raycastHit; 

        bool hit =  Physics.Raycast(ray, out raycastHit, rayDistance, ground);

        return hit;
    }

    [Command]
    public void SetWheelSpeed(float speed)
    {
        frontWheel.SetMaxSpeed(speed);
        backWheel.SetMaxSpeed(speed);
    }

    [Command]
    public void SetTireGrip(float grip)
    {
        frontWheel.SetTireGrip(grip);
        backWheel.SetTireGrip(grip);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawLine(frontWheel.transform.position, frontWheel.transform.position + Vector3.down * rayDistance);
        Gizmos.DrawLine(backWheel.transform.position, backWheel.transform.position + Vector3.down * rayDistance);
    }
}
