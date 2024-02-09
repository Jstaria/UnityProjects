using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObj = new();

    /// <summary>
    /// Add object at specified positions
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="objSize"></param>
    /// <param name="ID"></param>
    /// <param name="objectIndex"></param>
    /// <exception cref="Exception">Will not place if there is already an object here</exception>
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objSize, int ID, int objectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, objectIndex);

        foreach (Vector3Int pos in positionToOccupy)
        {
            if (placedObj.ContainsKey(pos))
            {
                throw new Exception($"Dict already contains {pos}");
            }

            placedObj[pos] = data;
        }
    }

    /// <summary>
    /// Returns total positions of object x by y grid spots
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="objSize"></param>
    /// <returns></returns>
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = 0; x < objSize.x; x++)
        {
            for (int y = 0; y < objSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, y, 0));
            }
        }

        return returnVal;
    }

    /// <summary>
    /// Returns if you can place object at a position via the dict of grid positions
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="objSize"></param>
    /// <returns></returns>
    public bool CanPlaceObjAt(Vector3Int gridPosition, Vector2Int objSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objSize);
        
        foreach (Vector3Int pos in positionToOccupy)
        {
            if (placedObj.ContainsKey(pos))
            {
                return false;
            }
        }

        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (!placedObj.ContainsKey(gridPosition))
        {
            return -1;
        }

        return placedObj[gridPosition].PlacedObjectIndex;
    }

    public int GetDatabaseIndex(Vector3Int gridPosition)
    {
        if (!placedObj.ContainsKey(gridPosition))
        {
            return -1;
        }

        return placedObj[gridPosition].ID;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (Vector3Int pos in placedObj[gridPosition].occupiedPositions)
        {
            placedObj.Remove(pos);
        }
    }

    public void SetPathObjects(List<Vector3Int> positions)
    {
        foreach (Vector3Int pos in positions)
        {
            List<Vector3Int> posi = new List<Vector3Int> { pos };
            placedObj.Add(pos, new PlacementData(posi, 3, placedObj.Count - 1));
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;

    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
