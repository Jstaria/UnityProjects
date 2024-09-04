using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // speed
    // object moving
    // direction
    // position

    private Vector3 position;
    private Vector3 direction;
    private Vector3 vel;
    internal Vector3 Direction { get { return direction; } set { direction = value.normalized; } }

    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        vel = direction * speed;

        GetComponent<Rigidbody2D>().velocity = vel;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, vel);
    }
}
