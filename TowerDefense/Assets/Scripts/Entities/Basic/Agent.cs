
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected PhysicsObject phyObj;
    //[SerializeField] protected ObstacleManager obstacleManager;
    [SerializeField] protected float maxSpeed = 10;
    [SerializeField] protected float maxForce = 20;
    [SerializeField] protected float avoidTime = 1;

    protected Vector3 ultimaForce;
    protected List<Vector3> foundObstacles = new List<Vector3>();

    public Vector3 UltimaForce { set { ultimaForce = value; } }


    // Update is called once per frame
    void Update()
    {
        //ultimaForce = Vector3.zero;

        CalcSteeringForce();

        Vector3.ClampMagnitude(ultimaForce, maxForce);
        phyObj.ApplyForce(ultimaForce);

    }

    protected Vector3 Seek(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - gameObject.transform.position;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - phyObj.Velocity;

        // Return seek steering force
        return seekingForce;
    }

    protected Vector3 Seek(GameObject target)
    {
        // Call the other version of Seek 
        //   which returns the seeking steering force
        //  and then return that returned vector. 
        return Seek(target.transform.position);
    }

    protected Vector3 Flee(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = gameObject.transform.position - targetPos;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate flee steering force
        Vector3 fleeingForce = desiredVelocity - phyObj.Velocity;

        // Return flee steering force
        return fleeingForce;
    }

    protected Vector3 Flee(GameObject target)
    {
        // Call the other version of Flee 
        //   which returns the seeking steering force
        //  and then return that returned vector. 
        return Flee(target.transform.position);
    }

    protected Vector3 Wander(ref float wanderAngle, float maxWanderChangePerSecond, float time, float maxWanderAngle, float radius)
    {
        float maxWanderChange = maxWanderChangePerSecond * Time.deltaTime;

        wanderAngle += Random.Range(-maxWanderChange, maxWanderChange);

        wanderAngle = Mathf.Clamp(wanderAngle, -maxWanderAngle, maxWanderAngle);

        // Get position that is defined by the wander angle
        Vector3 wanderTarget = Quaternion.Euler(0, 0, wanderAngle) * phyObj.Direction + transform.position;

        // Seek towards that wander target
        return Seek(wanderTarget);
    }


    public Vector3 CalcFuturePosition(float time)
    {
        return phyObj.Velocity * time + transform.position;
    }

    public Vector3 StayInBounds(float time, float outerBounds)
    {
        Vector3 position = phyObj.Position;//CalcFuturePosition(time);

        //if (position.x > totalCamWidth ||
        //    position.x < -totalCamWidth ||
        //    position.y > totalCamHeight ||
        //    position.y < -totalCamHeight)
        //{
        //    return Seek(Vector3.zero);
        //}


        return Vector3.zero;
    }

    protected abstract void CalcSteeringForce();
}
