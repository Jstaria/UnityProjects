using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    // The movement controller to update
    [SerializeField] private MovementController movCon;

    private Vector3 inputDir;


    private void Update()
    {
        inputDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) inputDir += new Vector3(.5f, 0, .5f);
        if (Input.GetKey(KeyCode.A)) inputDir += new Vector3(-.5f, 0, .5f);
        if (Input.GetKey(KeyCode.S)) inputDir += new Vector3(-.5f, 0, -.5f);
        if (Input.GetKey(KeyCode.D)) inputDir += new Vector3(.5f, 0, -.5f);

        // Send new direction
        movCon.Direction = inputDir;
    }

    // Update is called once per frame
    public void OnMove(InputAction.CallbackContext context)
    {
        // Get value from input system

        
    }
}
