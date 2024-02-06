using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObj = new();

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

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = 0; x < objSize.x; x++)
        {
            for (int y = 0; y < objSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }

        return returnVal;
    }

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
