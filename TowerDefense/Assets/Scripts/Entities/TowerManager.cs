using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private PlacementSystem placementSystem;

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<Vector3Int, PlacementData> pair in placementSystem.gridData.PlacedObj)
        {
            if (pair.Value.Tower == null) continue;
            pair.Value.Tower.Update();
        }
    }
}
