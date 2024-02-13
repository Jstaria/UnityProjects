using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    // The movement controller to update
    [SerializeField] private MovementController movCon;

    private Vector3 inputDir;
    

    // Update is called once per frame
    public void OnMove(InputAction.CallbackContext context)
    {
        // Get value from input system
        inputDir = context.ReadValue<Vector2>();

        inputDir.z = inputDir.y;
        inputDir.y = 0;

        // Send new direction
        movCon.Direction = inputDir;
    }
}
