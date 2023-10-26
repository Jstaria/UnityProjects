using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassBehavior : MonoBehaviour
{
    [SerializeField] private float radius = .5f;

    private Vector2 position;
    private Vector2 direction;
    private Vector2 velocity;

    // Sum of all forces in a frame - New
    private Vector2 acceleration;

    // Mass of object - New
    [SerializeField] private float mass;

    [SerializeField] private float maxSpeed;

    public float Radius { get { return radius; } }
    public float MaxSpeed { set { maxSpeed = value ; } }
    public float Mass { set { mass = value; } }

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the velocity for this frame - New
        velocity += Vector2.ClampMagnitude(acceleration * Time.deltaTime, maxSpeed);

        position += velocity * Time.deltaTime;

        // Grab current direction from velocity  - New
        direction = velocity.normalized;

        transform.position = position;

        // Zero out acceleration - New
        acceleration = Vector3.zero;

    }

    void ApplyGravity(Vector2 force)
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

    public void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    }

    public void SetVel(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public Vector2 GetAttractionForce(Vector2 position)
    {
        Vector2 force = Vector2.zero;

        force = ((Vector2)transform.position - position);
        force = force.normalized * Mathf.Pow(2, (-force.magnitude / 100) + 10) * mass;

        return force;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);

        //Camera.main.orthographicSize = 20;
        //Camera.main.transform.position = transform.position + new Vector3(0,0,-10);
    }
}
