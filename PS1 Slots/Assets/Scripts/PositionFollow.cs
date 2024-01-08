using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollow : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 offset;

    public Vector3 Offset { get { return offset; } set { offset = value; } }

    // Update is called once per frame
    void Update()
    {
        transform.position = Target.position + offset;
    }
}
