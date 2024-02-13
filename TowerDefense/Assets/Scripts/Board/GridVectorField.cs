using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GridVectorField : MonoBehaviour
{
    [SerializeField] private DisplayVectorField display;
    private Dictionary<Vector3Int, Vector3Int> fieldPositionValues = new();
    private Dictionary<Vector3Int, Vector3Int> pathValues = new();

    //[SerializeField] int outOfBoundsScale = 5;

    public Dictionary<Vector3Int, Vector3Int> FieldPositionValues { get { return fieldPositionValues; } }
    public Dictionary<Vector3Int, Vector3Int> PathValues { get { return pathValues; } }

    public void Display()
    {
        display.Display();
    }

    public void SetVector(Vector3Int gridPosition, Vector3Int vectorDirection)
    {
        gridPosition = new Vector3Int(gridPosition.x, 0, gridPosition.y);

        if (CheckValidity(gridPosition))
        {
            fieldPositionValues[gridPosition] = vectorDirection;
        }
        else fieldPositionValues.Add(gridPosition, vectorDirection);
    }

    public void SetAdjacentVectors()
    {
        List<Vector3Int> directions = new List<Vector3Int>()
        {
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 0, -1),
        };

        pathValues.AddRange(fieldPositionValues);

        int count = fieldPositionValues.Count;

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector3Int currentAdjacency = fieldPositionValues.ElementAt(i).Key + directions[j];

                if (CheckValidity(currentAdjacency)) continue;

                fieldPositionValues.Add(currentAdjacency, -directions[j]);
            }
        }
    }

    private bool CheckValidity(Vector3Int gridPosition)
    {
        return fieldPositionValues.ContainsKey(gridPosition);
    }

    public Vector3Int GetVector(Vector3Int gridPosition)
    {
        gridPosition = new Vector3Int(gridPosition.x, 0, gridPosition.y);

        return CheckValidity(gridPosition) ? fieldPositionValues[gridPosition] : Vector3Int.zero;
    }

    //private void OnDrawGizmos()
    //{
    //    foreach (KeyValuePair<Vector3Int,Vector3Int> pair in fieldPositionValues)
    //    {
    //        Gizmos.DrawCube(pair.Key + new Vector3(.5f, 0, .5f), Vector3.one / 5);
    //    }
    //}
}
