using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : Singleton<CameraArm>
{
    [SerializeField] private Transform pedestalTransform;

    [SerializeField] private Vector3 offset = new Vector3(1,0,0);

    [SerializeField] private Camera cam;

    public Vector3 desiredPosition;
    public Vector3 desiredRotation;

    private float lerpSpeed = 4f;

    private void Update()
    {
        float blend = MathF.Pow(.5f, Time.deltaTime * lerpSpeed);

        transform.position = Vector3.Slerp(desiredPosition, transform.position, blend);

        blend = MathF.Pow(.5f, Time.deltaTime * (lerpSpeed + .5f));

        transform.rotation = Quaternion.Slerp(Quaternion.Euler(desiredRotation), transform.rotation, blend);
    }
}
