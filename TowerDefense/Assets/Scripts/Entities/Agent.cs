
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

    //public ObstacleManager ObstacleManager { set { obstacleManager = value; } }

    [SerializeField] private BoundsInfo floorBound;

    public BoundsInfo FloorBounds { get { return floorBound; } set { floorBound = value; } }

    // Update is called once per frame
    void Update()
    {
        //ultimaForce = Vector3.zero;

        CalcSteeringForce();

        Vector3.ClampMagnitude(ultimaForce, maxForce);
        phyObj.ApplyForce(ultimaForce);

        //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, MathF.Sin(phyObj.Direction.y) / MathF.Cos(phyObj.Direction.x));
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

    protected Vector3 AvoidObtacles()
    {
        foundObstacles.Clear();

        Vector3 totalForce = Vector3.zero;

        // Do Stuff
        //foreach (Obstacle obstacle in obstacleManager.Obstacles)
        //{
        //    // Checks for if I care
        //    Vector3 agentToObstacle = obstacle.transform.position - transform.position;
        //    float rightDot = 0, forwardDot = 0;

        //    float dist = Vector3.Distance(transform.position, CalcFuturePosition(avoidTime)) + phyObj.Radius;

        //    forwardDot = Vector3.Dot(phyObj.Direction, agentToObstacle);

        //    if (forwardDot >= -obstacle.Radius)
        //    {
        //        if (forwardDot <= dist + obstacle.Radius)
        //        {
        //            // How far left and rights
        //            rightDot = Vector3.Dot(transform.right, agentToObstacle);

        //            Vector3 steeringForce = transform.right * (1 - forwardDot / dist) * 10;

        //            if (Mathf.Abs(rightDot) <= phyObj.Radius + obstacle.Radius)
        //            {
        //                // If I care
        //                foundObstacles.Add(obstacle.transform.position);

        //                // If left
        //                if (rightDot <= 0)
        //                    totalForce += steeringForce;

        //                // If Right
        //                else
        //                {
        //                    totalForce -= steeringForce;
        //                }
        //            }
        //        }
        //    }
        //}

        return totalForce;
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

        if (position.x > FloorBounds.xMax - outerBounds)
        {
            return Seek(new Vector3(FloorBounds.xMax - outerBounds, transform.position.y));
        }
        if (position.x < FloorBounds.xMin + outerBounds)
        {
            return Seek(new Vector3(FloorBounds.xMin + outerBounds, transform.position.y));
        }

        if (position.y > FloorBounds.yMax - outerBounds)
        {
            return Seek(new Vector3(transform.position.x, FloorBounds.yMax - outerBounds));
        }
        if (position.y < FloorBounds.yMin + outerBounds)
        {
            return Seek(new Vector3(transform.position.x, FloorBounds.yMin + outerBounds));
        }

        return Vector3.zero;
    }

    protected abstract void CalcSteeringForce();
}
