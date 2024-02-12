using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileNode
{
    private Vector3Int position;
    private TileNode previousNode;
    private TileNode nextNode;
    private float radius;

    public Vector3Int BoardPosition { get { return position; } }
    public Vector3 Position { get { return position + new Vector3(.5f, 0, .5f); } }
    public TileNode PreviousNode { get { return previousNode; } }
    public TileNode NextNode { get { return nextNode; } set { nextNode = value; } }

    public TileNode(Vector3Int position, TileNode previousNode, float radius)
    {
        this.position = position;
        this.previousNode = previousNode;
        this.radius = radius;
    }

    public bool IsColliding(Vector3 position, float radius)
    {
        return (position - Position).magnitude <= radius + this.radius;
    }
}
