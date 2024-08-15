using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraArm : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Transform followTransform;
    public float lerpSpeed = 10;

    public override void OnNetworkSpawn()
    {
        cameraHolder.SetActive(IsOwner);
    }

    private void Update()
    {
        if (!IsOwner) return;
        MoveCamera();
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, followTransform.position + new Vector3(0, 0, transform.position.z), lerpSpeed * Time.deltaTime);
    }
}
