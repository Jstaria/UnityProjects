
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private PlayerStats playerStats;

    private Vector2 XYRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Cursor.lockState = QuantumConsole.Instance.IsActive ? CursorLockMode.None : CursorLockMode.Locked;
        //Cursor.visible = QuantumConsole.Instance.IsActive;

        Vector2 MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        XYRotation.x -= MouseInput.y * playerStats.sensitivity.y;
        XYRotation.y += MouseInput.x * playerStats.sensitivity.x;

        XYRotation.x = Mathf.Clamp(XYRotation.x, -75f, 75f);

        transform.eulerAngles = new Vector3(0f, XYRotation.y);
        PlayerCamera.localEulerAngles = new Vector3(XYRotation.x, 0f);
    }
}
