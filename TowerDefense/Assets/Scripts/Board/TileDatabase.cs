using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// List of our objects with data
/// </summary>
[CreateAssetMenu]
public class TileDatabase : ScriptableObject
{
    public List<TileData> tilesData;
}

/// <summary>
/// Holds the Name, ID, Size, and Object data
/// </summary>
[Serializable]
public class TileData
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Tile Tile { get; private set; }

}