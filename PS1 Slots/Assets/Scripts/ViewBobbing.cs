using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionFollow))]

public class ViewBobbing : MonoBehaviour
{
    [SerializeField] private float effectIntensity;
    [SerializeField] private float effectIntensityX;
    [SerializeField] private float effectSprintIntensityX;
    [SerializeField] private float effectSpeed;
    [SerializeField] private float effectSprintSpeed;

    private PositionFollow followTarget;
    private Vector3 originalOffset;
    private float sinTime;
    private float currentEffectSpeed;
    private float currentIntensityX;

    void Start()
    {
        followTarget = GetComponent<PositionFollow>();
        originalOffset = followTarget.Offset;
    }

    void Update()
    {
        Vector3 PlayerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentEffectSpeed = effectSprintSpeed;
            currentIntensityX = Mathf.Lerp(currentIntensityX, effectSprintIntensityX, .1f); ;
        }
        else 
        { 
            currentEffectSpeed = effectSpeed; 
            currentIntensityX = Mathf.Lerp(currentIntensityX, effectIntensityX, .1f); 
        }

        if (PlayerInput.magnitude > 0)
        {
            sinTime += Time.deltaTime * currentEffectSpeed;
            sinTime %= Mathf.PI * 2;
        }
        else
        {
            if (sinTime >= Mathf.PI)
            {
                sinTime = Mathf.Lerp(sinTime, Mathf.PI * 2, .05f);
            }
            else if (sinTime < Mathf.PI)
            {
                sinTime = Mathf.Lerp(sinTime, 0, .05f);
            }

            followTarget.Offset = Vector3.Lerp(followTarget.Offset, originalOffset, .05f);
        }

        float sinAmountY = -Mathf.Abs(effectIntensity * Mathf.Sin(sinTime));
        Vector3 sinAmountX = followTarget.transform.right * effectIntensity * Mathf.Cos(sinTime) * currentIntensityX;

        followTarget.Offset = new Vector3
        {
            x = originalOffset.x,
            y = originalOffset.y + sinAmountY,
            z = originalOffset.z
        };

        followTarget.Offset += sinAmountX;
    }
}
