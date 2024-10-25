using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public delegate void CameraFollowDelegate(Vector3 follow);
    public event CameraFollowDelegate OnFollow;

    void Update()
    {
        OnFollow.Invoke(transform.position);
    }
}
