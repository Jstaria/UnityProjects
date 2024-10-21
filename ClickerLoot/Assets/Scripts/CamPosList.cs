using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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
