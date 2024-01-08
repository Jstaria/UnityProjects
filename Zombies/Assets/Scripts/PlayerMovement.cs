using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSmoothTime;
    [SerializeField] private float gravityStrength;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private LayerMask whatIsGround;

    private CharacterController charCon;
    private Vector3 currentMoveVelocity;
    private Vector3 moveDampVelocity;
    private Vector3 currentForceVelocity;

    public bool IsGrounded { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerInput = new Vector3
        (
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        Vector3 moveVector = transform.TransformDirection(PlayerInput);
        float CurrentSpeed = Input.GetKey(KeyCode.LeftControl) ? runSpeed : walkSpeed;

        currentMoveVelocity = Vector3.SmoothDamp
        (
            currentMoveVelocity,
            moveVector * CurrentSpeed,
            ref moveDampVelocity,
            moveSmoothTime
        );

        charCon.Move(currentMoveVelocity * Time.deltaTime);

        // Check for ground
        Ray groundCheckRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(groundCheckRay, 1.1f, whatIsGround))
        {
            currentForceVelocity.y = -2f;
            IsGrounded = true;

            if (Input.GetKey(KeyCode.Space))
            {
                currentForceVelocity.y = jumpStrength;
                IsGrounded = false;
            }
        }
        else
        {
            currentForceVelocity.y -= gravityStrength * Time.deltaTime;
        }

        charCon.Move(currentForceVelocity * Time.deltaTime);
    }
}
