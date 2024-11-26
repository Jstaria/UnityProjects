using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FOV
{
    Rest,
    Sprint
}

public class CameraFovController : MonoBehaviour
{
    private Spring spring;

    [SerializeField] private float angularFrequency;
    [SerializeField] private float dampingRatio;
    [SerializeField] private float restFOV;
    [SerializeField] private float sprintFOV;

    [SerializeField] private List<Camera> cameras;

    private FOV currentFOV = FOV.Rest;

    // Start is called before the first frame update
    void Start()
    {
        spring = new Spring(angularFrequency, dampingRatio, restFOV, true);
    }

    // Update is called once per frame
    void Update()
    {
        float targetFOV = 0;

        switch (currentFOV)
        {
            case FOV.Rest:
                targetFOV = restFOV; break;

            case FOV.Sprint:
                targetFOV = sprintFOV; break;
        }

        spring.RestPosition = targetFOV;
        
        spring.Update();

        foreach (Camera cam in cameras)
        {
            cam.fieldOfView = spring.Position;
        }
    }

    [Command]
    public void SetFOV(FOV fov)
    {
        currentFOV = fov;
    }
}
