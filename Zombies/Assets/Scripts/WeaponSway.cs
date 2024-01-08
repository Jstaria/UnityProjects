using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private float swayAmount = 1f;
    [SerializeField] private float aimSwayAmount = .05f;
    [SerializeField] private float swaySmoothAmount = 6;
    [SerializeField] private float maxAmount = 1;
    [SerializeField] private float movementSwayStrength = 1;

    [Header("Rotation")]
    [SerializeField] private float rotationAmount = 4f;
    [SerializeField] private float maxRotationAmount = 5f;
    [SerializeField] private float smoothRotation = 12f;

    [Space]
    [SerializeField] private bool rotationX = true;
    [SerializeField] private bool rotationY = true;
    [SerializeField] private bool rotationZ = true;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private CharacterController playerCon;
    private GameObject player;
    private float currentSwayAmount;

    private float inputX;
    private float inputY;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        player = GameObject.Find("Player");
        playerCon = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        CalcSway();

        MoveSway();

        TiltSway();
    }

    private void CalcSway()
    {
        inputX = -Input.GetAxis("Mouse X");
        inputY = -Input.GetAxis("Mouse Y");
    }

    private void MoveSway()
    {
        this.currentSwayAmount = Input.GetMouseButton(1) ? aimSwayAmount : swayAmount;

        float moveX = Mathf.Clamp(inputX * currentSwayAmount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(inputY * currentSwayAmount, -maxAmount, maxAmount);

        if (!player.GetComponent<PlayerMovement>().IsGrounded)
        {
            moveY += Mathf.Clamp(-playerCon.velocity.y * movementSwayStrength, -maxAmount, maxAmount);
        }

        Vector3 finalOffset = new Vector3(moveX, moveY, 0);
        float lerpSpeed = Time.deltaTime * swaySmoothAmount;

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalOffset + initialPosition, lerpSpeed);
    }

    private void TiltSway()
    {
        float tiltX = Mathf.Clamp(inputY * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltY = Mathf.Clamp(inputX * rotationAmount, -maxRotationAmount, maxRotationAmount);

        if (!player.GetComponent<PlayerMovement>().IsGrounded)
        {
            tiltY += Mathf.Clamp(-playerCon.velocity.y * movementSwayStrength, -maxRotationAmount, maxRotationAmount);
        }

        Quaternion finalRotation = Quaternion.Euler(
                rotationX ? -tiltX : 0f, 
                rotationY ? tiltY : 0f, 
                rotationZ ? tiltY : 0f
            );

        float lerpSpeed = Time.deltaTime * smoothRotation;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, lerpSpeed);
    }
}
