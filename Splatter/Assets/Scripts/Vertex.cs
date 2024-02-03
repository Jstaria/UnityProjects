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
