using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1;

    [SerializeField] private Vector2 maxTilt = Vector2.one;

    [SerializeField] private Transform itemTransform;
    private Vector3 rotation;

    private void Start()
    {

    }

    private void Update()
    {
        if (itemTransform == null) return;

        rotation = new Vector3(
            Mathf.Sin(Time.time * rotationSpeed * 2) * maxTilt.x, 
            Mathf.Cos(Time.time * rotationSpeed) * maxTilt.y, 
            Mathf.Sin(Time.time * rotationSpeed * 2) * Mathf.Cos(Time.time * rotationSpeed) * maxTilt.x);
        itemTransform.rotation = Quaternion.Euler(rotation);
    }
}
