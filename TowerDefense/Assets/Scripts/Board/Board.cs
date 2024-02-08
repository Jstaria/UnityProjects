using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileDatabase tileDatabase;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject gridVisual;

    private TileNode rootNode;
    private Vector2Int startingTilePos;

    private float currentNode = 0;
    [SerializeField] private float nodeCount = 0;

    private void Start()
    {
        Vector3 scale = new Vector3(width * .1f, 1, height * .1f);
        Vector3 offset = new Vector3(width % 2 == 0 ? 0 : .5f, 0, height % 2 == 0 ? 0 : .5f);

        gridVisual.transform.localScale = scale;
        //gridPlane.transform.localScale = scale;

        gridVisual.transform.position += offset;
        //gridPlane.transform.position += offset;

        GenerateBoard();
    }

    public void GenerateBoard()
    {
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

        rootNode = new TileNode(new Vector3Int(startingX, 0, startingY), null);

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

            while (!CheckPositionValidity(currentNode.Position + direction * numberOfTilesInDir) || direction == -prevDir)
            {
                if (stuckCount < 4 || numberOfTilesInDir > 1)
                {
                    numberOfTilesInDir--;
                    stuckCount++;
                }
                else
                {
                    numberOfTilesInDir = Random.Range(3, 15);
                    direction = directions[Random.Range(0, directions.Length)];
                }
                
            }

            tempNode = new TileNode(currentNode.Position + direction * numberOfTilesInDir, currentNode);

            currentNode.NextNode = tempNode;
            currentNode = tempNode;
            prevDir = direction;
        }

    }

    private void PlacePathTiles()
    {
        TileNode currentNode = rootNode;

        while (currentNode.NextNode != null)
        {
            Vector3Int DirectionScaled = currentNode.NextNode.Position - currentNode.Position;

            if (DirectionScaled.x > 0 || DirectionScaled.z > 0)
            {
                if (DirectionScaled.x == 0)
                {
                    for (int i = 0; i < DirectionScaled.z; i++)
                    {
                        tileMap.SetTile(new Vector3Int(currentNode.Position.x, currentNode.Position.z) + new Vector3Int(0, i), tileDatabase.tilesData[0].Tile);
                    }
                }
                if (DirectionScaled.z == 0)
                {
                    for (int i = 0; i < DirectionScaled.x; i++)
                    {
                        tileMap.SetTile(new Vector3Int(currentNode.Position.x, currentNode.Position.z) + new Vector3Int(i, 0), tileDatabase.tilesData[0].Tile);
                    }
                }
            }
            else
            {
                if (DirectionScaled.x == 0)
                {
                    for (int i = 0; i < Mathf.Abs(DirectionScaled.z); i++)
                    {
                        tileMap.SetTile(new Vector3Int(currentNode.Position.x, currentNode.Position.z) - new Vector3Int(0, i), tileDatabase.tilesData[0].Tile);
                    }
                }
                if (DirectionScaled.z == 0)
                {
                    for (int i = 0; i < Mathf.Abs(DirectionScaled.x); i++)
                    {
                        tileMap.SetTile(new Vector3Int(currentNode.Position.x, currentNode.Position.z) - new Vector3Int(i, 0), tileDatabase.tilesData[0].Tile);
                    }
                }
            }
            

            currentNode = currentNode.NextNode;
        }
    }

    private bool CheckPositionValidity(Vector3Int position)
    {
        return 
            position.x >= startingTilePos.x + 1 && position.z >= startingTilePos.y + 1 &&
            position.x < width / 2 - 1 && position.z < height / 2 - 1;
    }

    private void OnDrawGizmos()
    {
        currentNode = 0;
        TileNode current = rootNode;
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

            Gizmos.DrawSphere(current.Position, .5f);

            if (current.NextNode != null)
            {
                Gizmos.color = Color.Lerp(Color.cyan,Color.magenta, currentNode / nodeCount);
                Gizmos.DrawLine(current.Position, current.NextNode.Position);
            }

            currentNode += 1;

            current = current.NextNode;
        }
    }
}
