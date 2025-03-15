using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    Idle,
    Walking,
    Sprinting
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Rigidbody2D rb;

    public MovementState CurrentState {  get; private set; }
    public Vector2 Direction { get { return direction; } }

    private Vector2 direction;
    private float movementSpeed;
    private float acceleratedSpeed;

    private void Start() => CurrentState = MovementState.Idle;

    public void MovementUpdate()
    {
        Vector2 direction = GetMovementDirection();
        GetMovementSpeed();

        Move(direction * movementSpeed);
    }

    private void Move(Vector3 vector2) => transform.position += vector2;

    private void GetMovementSpeed()
    {
        movementSpeed = playerStats.playerWalkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && CurrentState != MovementState.Idle)
        {
            CurrentState = MovementState.Sprinting;
            movementSpeed = playerStats.playerRunSpeed;
        }
    }

    private Vector2 GetMovementDirection()
    {
        CurrentState = MovementState.Idle;

        //float accel = 0;
        float lerp = playerStats.acceleration;
        bool movementFlagX = false;
        bool movementFlagY = false;

        if (Input.GetKey(KeyCode.W)) { direction.y = Mathf.Lerp(+1, direction.y, lerp); movementFlagY = true; }
        if (Input.GetKey(KeyCode.A)) { direction.x = Mathf.Lerp(-1, direction.x, lerp); movementFlagX = true; }
        if (Input.GetKey(KeyCode.S)) { direction.y = Mathf.Lerp(-1, direction.y, lerp); movementFlagY = true; }
        if (Input.GetKey(KeyCode.D)) { direction.x = Mathf.Lerp(+1, direction.x, lerp); movementFlagX = true; }
        
        if (movementFlagX || movementFlagY)
            CurrentState = MovementState.Walking;
        if (!movementFlagX)
            direction.x = Mathf.Lerp(0, direction.x, lerp);
        if (!movementFlagY)
            direction.y = Mathf.Lerp(0, direction.y, lerp);

        //acceleratedSpeed = Mathf.Lerp(accel,acceleratedSpeed, lerp);

        return Direction;
    }
}
