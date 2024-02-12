using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private MovementController movCon;
    [SerializeField] private GameObject sprite;

    private GameObject texture;

    // Start is called before the first frame update
    void Start()
    {
        texture = Instantiate(sprite, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Aim(InputAction.CallbackContext context)
    {
        float rotationAngle = 0;

        transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
        Vector2 lookDirection = context.ReadValue<Vector2>();
        
        if (lookDirection.x == -1) { rotationAngle = 0; }
        if (lookDirection.x == 1) { rotationAngle = 180; transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z); }
        if (lookDirection.y == -1) { rotationAngle = 90; }
        if (lookDirection.y == 1) { rotationAngle = -90; }

        texture.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotationAngle));
        texture.transform.position = lookDirection + (Vector2)transform.position;
        Debug.Log(transform.rotation);
    }
}
