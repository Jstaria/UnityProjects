using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vertex
{
    private Vector2 position;
    private float data;

    public Vertex (Vector2 position, float data)
    {
        this.position = position;
        this.data = data;
    }

    public float Data { get { return data; } set { data = value; } }
    public Vector2 Position { get { return position; } set { position = value; } }
}

public struct Triangle
{
    private Vector2 point1, point2, point3;
    private int index1, index2, index3;

    public Triangle(Vector2 point1, Vector2 point2, Vector2 point3, int index1, int index2, int index3) 
    {
        this.point1 = point3;
        this.point2 = point2;
        this.point3 = point1;

        this.index1 = index1;
        this.index2 = index2;
        this.index3 = index3;
    }

    public List<Vector3> GetPoints()
    {
        return new List<Vector3>() { new Vector3(point1.x, point1.y), new Vector3(point2.x, point2.y), new Vector3(point3.x, point3.y) };
    }
    public List<int> GetIndices()
    {
        return new List<int>() { index1, index2, index3 };
    }
}
