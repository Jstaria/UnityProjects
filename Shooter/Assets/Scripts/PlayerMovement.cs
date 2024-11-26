using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QFSW.QC;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSmoothTime;
    [SerializeField] private float gravityStrength;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float momentumStrength;
    [SerializeField] private float maxMomentum;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private CameraFovController camFOVCon;

    private CharacterController charCon;
    private Vector3 currentMoveVelocity;
    private Vector3 moveDampVelocity;
    private Vector3 currentForceVelocity;

    private float currentMomentum;

    public bool IsGrounded { get; private set; }
    public bool IsSprinting { get; private set; }

    public delegate void OnJump();
    public event OnJump Jump;

    private bool RunDebug = false;
    private Coroutine debug;

    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<CharacterController>();

        Jump += AddJumpVelUp;
        Jump += AddJumpVelDir;
        Jump += ResetSlide;
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

        bool ctrlPressed = Input.GetKey(KeyCode.LeftControl);
        float CurrentSpeed = (ctrlPressed) ? runSpeed : walkSpeed;

        IsSprinting = ctrlPressed && moveVector.sqrMagnitude > 0;

        currentMoveVelocity = Vector3.SmoothDamp
        (
            currentMoveVelocity,
            moveVector * CurrentSpeed,
            ref moveDampVelocity,
            moveSmoothTime
        );

        camFOVCon.SetFOV((CurrentSpeed == walkSpeed ? FOV.Rest : ((moveVector == Vector3.zero) ? FOV.Rest : FOV.Sprint)));

        charCon.Move(currentMoveVelocity * Time.deltaTime);

        // Check for ground
        Ray groundCheckRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(groundCheckRay, 1.1f, whatIsGround))
        {
            currentForceVelocity.y = -2f;
            IsGrounded = true;

            if (Input.GetKey(KeyCode.Space))
            {
                Jump.Invoke();
            }
        }
        else
        {
            currentForceVelocity.y -= gravityStrength * Time.deltaTime;
        }

        if (IsGrounded)
        {
            currentForceVelocity = Vector3.Lerp(Vector3.up * -2, currentForceVelocity, Mathf.Pow(.5f, Time.deltaTime * 10));
        }
        else
        {
            Vector3 jumpMoveVel = currentMoveVelocity.normalized * currentMomentum;
            currentForceVelocity.x = jumpMoveVel.x;
            currentForceVelocity.z = jumpMoveVel.z;
        }

        charCon.Move(currentForceVelocity * Time.deltaTime);
    }

    private void ResetSlide()
    {
        SlideController.ResetSlide();
    }

    private void AddJumpVelUp()
    {
        currentForceVelocity.y = jumpStrength;
        IsGrounded = false;
    }

    private void AddJumpVelDir()
    {
        currentMomentum = Mathf.Clamp(currentMomentum + momentumStrength, 0, maxMomentum);
    }

    #region Debug

    [Command]
    public void PlayerDebug(bool start)
    {
        if (start && debug == null)
        {
            debug = StartCoroutine(DebugLog());
        }
        else
        {
            StopCoroutine(debug);
            debug = null;
        }
        
    }

    private IEnumerator DebugLog()
    {
        while(true)
        { 
            Debug.Log(currentForceVelocity);

            yield return new WaitForSeconds(.5f);
        }
    }

    #endregion
}
