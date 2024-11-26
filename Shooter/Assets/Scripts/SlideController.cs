using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    [SerializeField] private float slideStrength;

    [SerializeField] private PlayerMovement playerMov;
    [SerializeField] private CharacterController charCon;
    [SerializeField] private CrouchController crouchCon;

    private static float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void ResetSlide()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMov.IsSprinting) return;
        if (!playerMov.IsGrounded) return;

        //Debug.Log("CanSprint");

        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.C) && !crouchCon.IsCrouched && timer < 0)
        {
            timer = 1.0f;
            charCon.Move(playerMov.transform.forward * slideStrength * Time.deltaTime);
        }
    }
}
