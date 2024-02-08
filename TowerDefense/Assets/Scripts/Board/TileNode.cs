using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileNode
{
    private Vector3Int position;
    private TileNode previousNode;
    private TileNode nextNode;

    public Vector3Int Position { get { return position; } }
    public TileNode PreviousNode { get { return previousNode; } }
    public TileNode NextNode { get { return nextNode; } set { nextNode = value; } }

    public TileNode(Vector3Int position, TileNode previousNode)
    {
        this.position = position;
        this.previousNode = previousNode;
    }
}
