using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class ViewManager : Singleton<ViewManager>
{
    public CamPosList cam;

    private int currentPosition = 2;

    public void GoLeft()
    {
        currentPosition = Mathf.Clamp(currentPosition + 1, 1, cam.cameraPositions.Count - 1);
    }

    public void GoRight()
    {
        currentPosition = Mathf.Clamp(currentPosition - 1, 1, cam.cameraPositions.Count - 1);
    }

    public void SetPosition(string posName)
    {
        currentPosition = ThisPosition(posName);
    }

    private int ThisPosition(string posName)
    {
        for (int i = 0; i < cam.cameraPositions.Count; i++)
        {
            if (posName == cam.cameraPositions[i].Name)
            {
                return i;
            }
        }

        return currentPosition;
    }

    private bool IsThisPosition(string name)
    {
        return name == cam.cameraPositions[currentPosition].Name;
    }

    private void Update()
    {
        if (!IsThisPosition("Chest"))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                GoRight();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                GoLeft();
            }
        }
        
        CameraArm.Instance.desiredPosition = cam.cameraPositions[currentPosition].Position;
        CameraArm.Instance.desiredRotation = cam.cameraPositions[currentPosition].Rotation;
    }

}

#region CamPosList
[CreateAssetMenu]
public class CamPosList : ScriptableObject
{
    public List<CamPos> cameraPositions;
}

[Serializable]
public class CamPos
{
    public string Name;
    public Vector3 Position;
    public Vector3 Rotation;
}
#endregion