using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Vector2 Sensitivities;

    private Vector2 XYRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = QuantumConsole.Instance.IsActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = QuantumConsole.Instance.IsActive;

       Vector2 MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        XYRotation.x -= MouseInput.y * Sensitivities.y;
        XYRotation.y += MouseInput.x * Sensitivities.x;

        XYRotation.x = Mathf.Clamp(XYRotation.x, -75f, 75f);

        transform.eulerAngles = new Vector3(0f, XYRotation.y);
        PlayerCamera.localEulerAngles = new Vector3(XYRotation.x, 0f);
    }
}
