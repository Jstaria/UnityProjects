using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    private Vector3 position;
    private Vector3 direction;
    private Vector3 velocity;

    // For collisions later
    [SerializeField] private float radius;

    // Friction coeff
    [SerializeField] private float coeff;
    [SerializeField] private bool bounce;

    // Max Speed
    [SerializeField] private float maxSpeed = 10;

    // Sum of all forces in a frame - New
    [SerializeField] private Vector3 acceleration = Vector3.zero;

    // Mass of object - New
    [SerializeField] private float mass = 1;

    [SerializeField] private bool useGravity = false;
    [SerializeField] private bool useFriction = false;

    private Camera cam;
    private float totalCamHeight;
    private float totalCamWidth;

    public float Radius { get { return radius; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }

    public Vector3 Position { get { return position; } set { position = value; } }
    public Vector3 Direction { get { return direction; } set { direction = value; } }

    // Start is called before the first frame update
    void Start()
    {
        //velocity = Random.insideUnitCircle.normalized;
        //velocity.z = velocity.y;
        //velocity.y = 0;

        velocity = new Vector3(0, 0, 0);

        position = transform.position;

        cam = Camera.main;

        totalCamHeight = cam.orthographicSize;
        totalCamWidth = totalCamHeight * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (useGravity) { ApplyGravity(new Vector3(0, -9.8f)); }
        if (useFriction) { ApplyFriction(coeff); }
        
        // Calculate the velocity for this frame - New
        velocity += Vector3.ClampMagnitude(acceleration * Time.deltaTime, maxSpeed);

        position += velocity * Time.deltaTime;

        // Grab current direction from velocity  - New
        direction = velocity.normalized;

        transform.position = position;

        // Zero out acceleration - New
        acceleration = Vector3.zero;

        if (bounce)
        {
            ScreenBounds(position);
        }
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void ApplyGravity(Vector3 force)
    {
        acceleration += force;
    }

    void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        ApplyForce(friction);
    }

    private void ScreenBounds(Vector3 position)
    { 
        if (position.x + transform.localScale.x / 3 > totalCamWidth)
        {
            Mathf.Clamp(position.x, 0, totalCamWidth);
            velocity.x *= -1;
        }
        else if (position.x - transform.localScale.x / 3 < -totalCamWidth)
        {
            Mathf.Clamp(position.x, -totalCamWidth, 0);
            velocity.x *= -1;
        }

        if (position.y + transform.localScale.y / 3 > totalCamHeight)
        {
            Mathf.Clamp(position.y, 0, totalCamHeight);
            velocity.y *= -1;
        }
        else if (position.y - transform.localScale.y / 3 < -totalCamHeight)
        {
            Mathf.Clamp(position.y, -totalCamHeight, 0);
            velocity.y *= -1;
        }

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -totalCamWidth, totalCamWidth), Mathf.Clamp(transform.position.y, -totalCamHeight, totalCamHeight));
    }

    public Vector3 CalcFuturePosition(float time)
    {
        return velocity * time + position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Velocity);
    }
}
