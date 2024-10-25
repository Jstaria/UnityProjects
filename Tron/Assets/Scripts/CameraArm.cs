using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField] private GameObject cameraPrefab;

    private GameObject cam;

    [SerializeField] private float armLength;
    [SerializeField] private CameraFollow camFollow;

    // Start is called before the first frame update
    void Start()
    {
        camFollow.OnFollow += OnFollow;

        Camera[] cameras = Camera.allCameras;

        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        cam = Instantiate(cameraPrefab);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFollow(Vector3 follow)
    {
        cam.transform.position = follow + -Vector3.forward * armLength + Vector3.up * armLength / 1.5f;
        cam.transform.LookAt(follow);
    }
}
