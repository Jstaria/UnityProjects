using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int xLength;
    [SerializeField] private int yLength;
    [SerializeField] private GameObject tile;
    [SerializeField] private float tileScale;
    [SerializeField] private int maxPathLength;
    [SerializeField] private GameObject enemySpawner;

    private List<tileInfo> path;
    private GameObject[,] tileMap;

    internal List<tileInfo> Path { get { return path; } }

    void Start()
    {
        CreateBoard();

        SetUpBoard();

        enemySpawner = Instantiate(enemySpawner);
        enemySpawner.GetComponent<EnemySpawner>().Grid = this;
    }

    private void Update()
    {
        if (enemySpawner.GetComponent <EnemySpawner>().Enemies.Count == 0)
        {
            CreateBoard();

            SetUpBoard();

            enemySpawner = Instantiate(enemySpawner);
            enemySpawner.GetComponent<EnemySpawner>().Grid = this;
        }
    }

    private void SetUpBoard()
    {
        int endX = Random.Range(0, xLength);
        int endY = Random.Range(0, yLength);
        int startX = endX;
        int startY = endY;

        // Creating start
        path = new List<tileInfo>();
        tileMap[startX, startY].GetComponent<tileInfo>().TileState = TileState.Start;
        path.Add(tileMap[startX, startY].GetComponent<tileInfo>());

        maxPathLength = (int)(xLength * yLength * .1666);

        int count = maxPathLength;

        // Directions for the path to wind
        Vector2[] directions = new Vector2[]
        {
            new Vector2(0,1),
            new Vector2(1,0),
            new Vector2(0,-1),
            new Vector2(-1,0)
        };

        Vector2 prevDir = Vector2.zero;

        while (count > 0)
        {
            int stuck = 0;
            
            // Random amount traveled for path
            int randNum = Random.Range(3, 5);
            Vector2 direction;

            // Cannot have path travel the opposite way without going another direction first
            do { direction = directions[Random.Range(0, 4)]; }
            while (direction == -prevDir);

            while (randNum > 0)
            {   
                // Error checking the current path to see if there is another path in its imminent future
                bool breakFlag = false;

                for (int i = 1; i < randNum + 1; i++)
                {
                    if (path[path.Count - 1].x + direction.x * i < xLength && path[path.Count - 1].x + direction.x * i > 0
                    && path[path.Count - 1].y + direction.y * i < yLength && path[path.Count - 1].y + direction.y * i > 0)
                    {
                        if (tileMap[path[path.Count - 1].x + (int)direction.x * i, path[path.Count - 1].y + (int)direction.y * i].GetComponent<tileInfo>().TileState == TileState.Path)
                        {
                            breakFlag = true;
                            prevDir = direction;

                            // Redirection if there is another path
                            do { direction = directions[Random.Range(0, 4)]; }
                            while (direction == prevDir);

                            continue;
                        }
                    }
                }

                // If there is, it aborts the current trajectory and redirects
                if (breakFlag)
                {
                    stuck++;

                    // The program can only be stucka maximum amount of 4 times before *I* assume it cannot go on and is in an infinite loop
                    // Therefore, we then reset it
                    if (stuck == 4)
                    {
                        count = maxPathLength;
                        ResetPath();
                        stuck = 0;
                    }

                    randNum--;

                    continue;
                }

                // Checking the bounds of the array with the direction
                if (path[path.Count - 1].x + direction.x < xLength && path[path.Count - 1].x + direction.x > 0
                    && path[path.Count - 1].y + direction.y < yLength && path[path.Count - 1].y + direction.y > 0
                    && tileMap[path[path.Count - 1].x + (int)direction.x, path[path.Count - 1].y + (int)direction.y].GetComponent<tileInfo>().TileState != TileState.Start)
                {
                    // If I fits, I sits
                    tileMap[path[path.Count - 1].x + (int)direction.x, path[path.Count - 1].y + (int)direction.y].GetComponent<tileInfo>().TileState = TileState.Path;
                    path.Add(tileMap[path[path.Count-1].x + (int)direction.x, path[path.Count - 1].y + (int)direction.y].GetComponent<tileInfo>());

                    count--;
                }
                randNum--;
            }

            prevDir = direction;
        }

        // Last tile becomes the ending tile
        tileMap[path[path.Count - 1].x, path[path.Count - 1].y].GetComponent<tileInfo>().TileState = TileState.End;
    }

    private void CreateBoard()
    {
        // Offset to center the board
        Vector2 offset = new Vector2(-tileScale * xLength / 2 + tileScale / 2, -tileScale * yLength / 2 + tileScale / 2);
        tileMap = new GameObject[xLength, yLength];

        // Setting up scale and positions
        for (int row = 0; row < xLength; row++)
        {
            for (int col = 0; col < yLength; col++)
            {
                tileMap[row, col] = Instantiate(tile);
                tileMap[row, col].transform.localScale = new Vector3(tileScale, tileScale, 0);
                tileMap[row, col].transform.position = offset + new Vector2(row * tileScale, col * tileScale);
                
                // Setting base information, every piece is a lawn for now
                tileMap[row, col].GetComponent<tileInfo>().TileState = TileState.Lawn;
                tileMap[row, col].GetComponent<tileInfo>().x = row;
                tileMap[row, col].GetComponent<tileInfo>().y = col;
            }
        }
    }

    /// <summary>
    /// Resets board and path for regeneration
    /// </summary>
    private void ResetPath()
    {
        // Resets the path
        path.Clear();

        // Recreating the board for baseline
        CreateBoard();

        int x = Random.Range(0, xLength);
        int y = Random.Range(0, yLength);

        // Creating new start
        path.Add(tileMap[x,y].GetComponent<tileInfo>());
        tileMap[x,y].GetComponent<tileInfo>().TileState = TileState.Start;
    }
}
