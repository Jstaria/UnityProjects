using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController charCon;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LayerMask ground;

    private Vector3 velocity, moveDampVelocity, forceVelocity;
    private bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();

        charCon.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, 1.2f, ground))
        {
            forceVelocity.y = -2f;
            onGround = true;

            if (Input.GetKey(KeyCode.Space))
            {
                forceVelocity.y = playerStats.jumpForce;
                onGround = false;
            }
        }
        else
        {
            float gravityStrength = 9.1f * 2;
            forceVelocity.y -= gravityStrength * Time.deltaTime;
        }

        if (onGround)
        {
            forceVelocity = Vector3.Lerp(Vector3.up * -2, forceVelocity, Mathf.Pow(.5f, Time.deltaTime * 10));
        }

        charCon.Move(forceVelocity * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 PlayerInput = new Vector3
         (
             Input.GetAxisRaw("Horizontal"),
             0f,
             Input.GetAxisRaw("Vertical")
         ).normalized;

        Vector3 moveVector = transform.TransformDirection(PlayerInput);

        bool ctrlPressed = Input.GetKey(KeyCode.LeftControl);
        float CurrentSpeed = (ctrlPressed) ? playerStats.playerRunSpeed : playerStats.playerWalkSpeed;


        velocity = Vector3.SmoothDamp
        (
            velocity,
            moveVector * CurrentSpeed,
            ref moveDampVelocity,
            playerStats.moveSmoothTime
        );
    }
}
