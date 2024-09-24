using System;
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

    internal Vector3 Velocity { get { return vel; } }
    internal Vector3 Direction { get { return direction; } set { direction = value.normalized; } }

    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator animator;

    [SerializeField] private Transform Up;
    [SerializeField] private Transform Down;
    [SerializeField] private Transform Right;

    [SerializeField] private Vector2 prevVel;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        prevVel = Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        vel = direction * speed;

        UpdateAnimator();

        GetComponent<Rigidbody2D>().velocity = vel;
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("XInput", vel.x);
        animator.SetFloat("YInput", vel.y);

        Up.gameObject.SetActive(false);
        Down.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);

        if (direction.y > 0 || prevVel.y > 0) Up.gameObject.SetActive(true);
        else if (direction.y < 0 || prevVel.y < 0) Down.gameObject.SetActive(true);
        else if ((direction.x > 0 && direction.y == 0) || (prevVel.x > 0 && prevVel.y == 0))
        {
            Right.gameObject.SetActive(true);
            Right.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (direction.x < 0 && direction.y == 0 || (prevVel.x < 0 && prevVel.y == 0))
        {
            Right.gameObject.SetActive(true);
            Right.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        if (vel != Vector3.zero) prevVel = vel;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, vel);
    }
}
