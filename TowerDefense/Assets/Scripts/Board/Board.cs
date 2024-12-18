using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileDatabase tileDatabase;
    private GridData gridData;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject gridVisual;

    private TileNode rootNode;
    private TileNode endNode;
    private int currentNodeCount = 1;
    private Vector2Int startingTilePos;

    [SerializeField] private float nodeCount = 0;

    [SerializeField] private GridVectorField gridVectorField;
    [SerializeField] private BoundsInfo boundsInfo;

    public Vector3 StartPosition { get { return rootNode.Position; }}
    public List<Vector3Int> TilePositions { get; private set; } = new List<Vector3Int>();
    public TileNode Root { get { return rootNode; } }

    private void Start()
    {
        Vector3 scale = new Vector3(width * .1f, 1, height * .1f);
        Vector3 offset = new Vector3(width % 2 == 0 ? 0 : .5f, 0, height % 2 == 0 ? 0 : .5f);

        gridVisual.transform.localScale = scale;
        //gridPlane.transform.localScale = scale;

        gridVisual.transform.position += offset;
        //gridPlane.transform.position += offset;

        boundsInfo.SetBounds(width, height);
    }

    public void GenerateBoard(GridData gridData)
    {
        this.gridData = gridData;
        startingTilePos = new Vector2Int(-width / 2, -height / 2); 

        for (int i = startingTilePos.x; i < width / 2; i++)
        {
            for (int j = startingTilePos.y; j < height / 2; j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j);
                tileMap.SetTile(tilePos, Instantiate(tileDatabase.tilesData[1].Tile));
            }
        }

        GeneratePathNodes((int)nodeCount);
        PlacePathTiles();
    }

    private void GeneratePathNodes(int numberOfNodes)
    {
        int startingX = Random.Range(startingTilePos.x + 2, width / 2 - 2);
        int startingY = Random.Range(startingTilePos.y + 2, height / 2 - 2);

        rootNode = new TileNode(new Vector3Int(startingX, 0, startingY), null, .1f);
        currentNodeCount = 1;

        TileNode currentNode = rootNode;

        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(-1, 0, 0),
            new Vector3Int( 0, 0, 1),
            new Vector3Int( 1, 0, 0),
            new Vector3Int( 0, 0,-1),
        };

        Vector3Int direction = directions[3];
        Vector3Int prevDir = Vector3Int.zero;
        int numberOfTilesInDir = 5;

        for (int i = 0; i < numberOfNodes; i++)
        {
            TileNode tempNode = null;

            if (i > 0)
            {
                direction = directions[Random.Range(0,directions.Length)];
                numberOfTilesInDir = Random.Range(3, 15);
            }

            int stuckCount = 0;
            int totalStuck = 0;
            while (!CheckPositionValidity(currentNode.BoardPosition + direction * numberOfTilesInDir, currentNode.BoardPosition, direction, numberOfTilesInDir, currentNodeCount) 
                || direction == -prevDir)
            {
                if (stuckCount < 4 || numberOfTilesInDir > 1)
                {
                    numberOfTilesInDir--;
                    stuckCount++;
                }
                else if (totalStuck > 20)
                {
                    return;
                }
                else
                {
                    numberOfTilesInDir = Random.Range(3, 15);
                    direction = directions[Random.Range(0, directions.Length)];
                    totalStuck++;
                }
                
            }

            tempNode = new TileNode(currentNode.BoardPosition + direction * numberOfTilesInDir, currentNode, .1f);

            currentNode.NextNode = tempNode;
            currentNode = tempNode;
            currentNodeCount++;
            endNode = tempNode;
            prevDir = direction;
        }

    }

    private void PlacePathTiles()
    {
        TileNode currentNode = rootNode;

        while (currentNode.NextNode != null)
        {
            Vector3Int directionScaled = currentNode.NextNode.BoardPosition - currentNode.BoardPosition;

            int stepX = (directionScaled.x > 0 ? 1 : -1);
            int stepZ = (directionScaled.z > 0 ? 1 : -1);

            if (directionScaled.x != 0)
            {
                for (int i = 0; i < Mathf.Abs(directionScaled.x); i++)
                {
                    Vector3Int position = new Vector3Int(currentNode.BoardPosition.x + (i * stepX), currentNode.BoardPosition.z);
                    tileMap.SetTile(position, tileDatabase.tilesData[0].Tile);
                    gridVectorField.SetVector(position, new Vector3Int(stepX, 0, 0));

                    if (!TilePositions.Contains(position))
                    {
                        TilePositions.Add(position);
                    }
                }
            }
            else if (directionScaled.z != 0)
            {
                for (int i = 0; i < Mathf.Abs(directionScaled.z); i++)
                {
                    Vector3Int position = new Vector3Int(currentNode.BoardPosition.x, currentNode.BoardPosition.z + (i * stepZ));
                    tileMap.SetTile(position, tileDatabase.tilesData[0].Tile);

                    gridVectorField.SetVector(position, new Vector3Int(0, 0, stepZ));

                    if (!TilePositions.Contains(position))
                    {
                        TilePositions.Add(position);
                    }
                }
            }

            currentNode = currentNode.NextNode;
        }

        gridData.SetPathObjects(TilePositions);
        gridVectorField.SetAdjacentVectors();
        gridVectorField.Display();
    }

    private bool CheckPositionValidity(Vector3Int nextPosition, Vector3Int currentPosition, Vector3Int direction, int scale, int nodeCount)
    {
        bool validity = 
            nextPosition.x >= startingTilePos.x + 1 && nextPosition.z >= startingTilePos.y + 1 &&
            nextPosition.x < width / 2 - 1 && nextPosition.z < height / 2 - 1 && nextPosition != rootNode.BoardPosition;

        if (nodeCount == this.nodeCount - 1)
        {
            TileNode tempNode = rootNode;

            while (tempNode != null)
            {
                TileNode tempNode1 = rootNode;

                Vector3Int position1 = tempNode.BoardPosition;

                while (tempNode1 != null)
                {
                    if (tempNode == tempNode1) { tempNode1 = tempNode1.NextNode; continue; }

                    Vector3Int position2 = tempNode1.BoardPosition;

                    Vector3Int directionScaled = position2 - position1;

                    int stepX = (directionScaled.x > 0 ? 1 : -1);
                    int stepZ = (directionScaled.z > 0 ? 1 : -1);

                    if (directionScaled.x != 0)
                    {
                        for (int i = 0; i < Mathf.Abs(directionScaled.x); i++)
                        {
                            Vector3Int currentPos = new Vector3Int(currentPosition.x + (i * stepX), 0, currentPosition.z);

                            if (nextPosition == currentPos) return false;
                        }
                    }
                    else if (directionScaled.z != 0)
                    {
                        for (int i = 0; i < Mathf.Abs(directionScaled.z); i++)
                        {
                            Vector3Int currentPos = new Vector3Int(currentPosition.x, 0, currentPosition.z + (i * stepZ));

                            if (nextPosition == currentPos) return false;
                        }
                    }

                    tempNode1 = tempNode1.NextNode;
                }

                tempNode = tempNode.NextNode;
            }
        }

        if (nodeCount == 1) return validity;

        for (int i = 0; i < scale; i++)
        {
            if (currentPosition + direction * i == rootNode.BoardPosition)
            {
                return false;
            }
            
        }

        return validity;
    }

    private void OnDrawGizmos()
    {
        int currentNode = 0;
        TileNode current = rootNode;
        Vector3 offset = new Vector3(.5f, 0, .5f);
        while (current != null)
        {
            Gizmos.color = Color.Lerp(Color.red, Color.blue, currentNode / nodeCount);

            if (current.PreviousNode == null)
            {
                Gizmos.color = Color.red;
            }
            else if (current.NextNode == null)
            {
                Gizmos.color = Color.blue;
            }

            Gizmos.DrawSphere(current.BoardPosition + offset, .5f);

            if (current.NextNode != null)
            {
                Gizmos.color = Color.Lerp(Color.red, Color.blue, currentNode / nodeCount);
                Gizmos.DrawLine(current.BoardPosition + offset, current.NextNode.BoardPosition + offset);
            }

            currentNode += 1;

            current = current.NextNode;
        }
    }
}
