using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGen : MonoBehaviour
{
    public List<GameObject> Rooms { get; set; }
    public GameObject previous { get; set; }
    public GameObject current { get; set; }

    private float prevSqrMag;

    public void GeneratePaths()
    {
       // Logic
    }
}
