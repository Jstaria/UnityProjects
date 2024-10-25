using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Acceleration : MonoBehaviour
{
    public delegate void AccelerationEvent(float angle);
    public event AccelerationEvent OnAccel;

    // Start is called before the first frame update
    void Update()
    {
        OnAccel.Invoke(GetAxis());
    }

    private float GetAxis()
    {
        float leftTrigger = Gamepad.current.leftTrigger.ReadValue();  // Returns value from 0 to 1
        float rightTrigger = Gamepad.current.rightTrigger.ReadValue();  // Returns value from 0 to 1

        float value = rightTrigger - leftTrigger;

        //Debug.Log(value);

        return value;
    }

    public void Accel(InputAction.CallbackContext context)
    {
        
    }
}
