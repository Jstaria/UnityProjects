using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]

public class TileManager : MonoBehaviour
{
    private int width;
    private int length;
    private int height;

    [SerializeField] private Vector3Int defaultTileDimensions = new Vector3Int(7, 7, 7);
    
    [SerializeField] private bool visualizeNoise = false;
    [SerializeField] private int offset = 6;
    [SerializeField] private float heightThreshhold = .5f;

    private float[,,] heights;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private List<List<string>> tileData = new List<List<string>>();

    private Vector3[,] tilePositions;
    private GameObject[,] tileObjects;
    private Vector2Int boardDimensions;

    private List<Vector3> cubePositions = new List<Vector3>();

    void Start()
    {
        width = defaultTileDimensions.x;
        height = defaultTileDimensions.y;
        length = defaultTileDimensions.z;

        List<string> boardData = FileIO.ReadFrom("TestBoard");

        string[] data = boardData[0].Split(',');

        boardDimensions = new Vector2Int(int.Parse(data[0]), int.Parse(data[1]));
        tilePositions = new Vector3[boardDimensions.x, boardDimensions.y];
        tileObjects = new GameObject[boardDimensions.x, boardDimensions.y];

        for (int i = 0; i < boardDimensions.x; i++)
        {
            for (int j = 0; j < boardDimensions.y; j++)
            {
                tilePositions[i, j] = new Vector3(-boardDimensions.x / 2 + i * offset, -boardDimensions.y / 2 + j * offset);
            }
        }

        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        UpdateAll();
    }

    private void UpdateAll()
    {
        SetHeights();
        MarchCubes();
        SetMesh();
    }

    private void SetHeights()
    {
        // Set the Dimensions of heights with 2 extra on each axis for the "air"/nothing cubes
        float[,,] tempHeights = new float[width * boardDimensions.x + 2, height + 2, length * boardDimensions.y + 2];
        heights = (float[,,])tempHeights.Clone();

        // Get the board data
        List<string> boardData = FileIO.ReadFrom("TestBoard");

        // Discern the tile types of the board, skip first since it is the board dimension
        for (int row = 0; row < boardDimensions.x; row++)
        {
            string[] data = boardData[row].Split(',');

            for (int col = 0; col < boardDimensions.y; col++)
            {
                string tile = data[col];

                // Instantiate tile class
                switch(tile)
                {
                    case "C":
                        // tileObjects[row,col] = Instantiate(crossPiece, tilePosition[row,col], Quaternion.Identity, transform);
                        break;
                }

                // Iterate through tile data to get height info
                int i = 1;
                int count = 0;
                // List<string> tileData = FileIO.ReadFrom(tileObjects[row,col].GetComponent<GameTileCreation>().FileName);
                List<string> tileData = FileIO.ReadFrom("Test");
                List<string> temp = new List<string>();

                // Create Lists for each layer of tile
                for (int j = 0; j < height + 1; j++)
                {
                    this.tileData.Add(new List<string>());
                }

                // Add Lists of tile data for the separate layers to their respective lists
                while (i < tileData.Count)
                {
                    if (tileData[i].Length <= 0)
                    {
                        this.tileData[count].AddRange(temp);
                        temp = new List<string>();
                        count++;
                        i++;
                        continue;
                    }

                    temp.Add(tileData[i]);
                    i++;
                }

                // Loop through each height vertex and set it in heights
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        data = this.tileData[y][x].Split(',');

                        for (int z = 0; z < length; z++)
                        {
                            tempHeights[x + 1 + width * row, y + 1, z + 1 + length * col] = float.Parse(data[z]);
                        }
                    }
                }
            }
        }
        
        for (int x = 0; x < heights.GetLength(0); x++)
        {
            for (int y = 0; y < heights.GetLength(1); y++)
            {
                for (int z = 0; z < heights.GetLength(2); z++)
                {
                    // Loop through each vertice and set the height to 0
                }
            }
        }

        // then copy over the level data shift 1 fromm each side using loops, hopefully this will allow the side geometry to actually show up
        // that's the goal anyway
    }

    private void MarchCubes()
    {
        vertices.Clear();
        triangles.Clear();

        for (int row = 0; row < boardDimensions.x; row++)
        {
            for (int col = 0; col < boardDimensions.y; col++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    for (int y = 0; y < height - 1; y++)
                    {
                        for (int z = 0; z < length - 1; z++)
                        {
                            float[] cubeCorners = new float[8];

                            for (int i = 0; i < 8; i++)
                            {
                                Vector3Int corner = new Vector3Int(x, y, z) + CubeTable.Corners[i];
                                cubeCorners[i] = heights[corner.x + 1, corner.y, corner.z + 1];
                            }

                            MarchCube(new Vector3(x + row * (width - 1), y, z + col * (length - 1)), cubeCorners);
                        }
                    }
                }
            }
        }
    }

    private void MarchCube(Vector3 position, float[] cubeCorners)
    {
        cubePositions.Add(position);
        int configIndex = GetConfigurationIndex(cubeCorners);

        // Will not generate cubes behind camera
        //float dotProduct = Vector3.Dot(position - cam.transform.position, cam.transform.forward);

        if (configIndex == 0 || configIndex == 255) //|| dotProduct < 0)
        {
            return;
        }

        int edgeIndex = 0;

        for (int t = 0; t < 5; t++)
        {
            for (int v = 0; v < 3; v++)
            {
                int triTableValue = CubeTable.Triangles[configIndex, edgeIndex];

                if (triTableValue == -1)
                {
                    return;
                }

                Vector3 edgeStart = position + CubeTable.Edges[triTableValue, 0];
                Vector3 edgeEnd = position + CubeTable.Edges[triTableValue, 1];

                Vector3 vertex;

                // Middle of the edge
                vertex = Vector3.Lerp(edgeStart, edgeEnd, (heightThreshhold - cubeCorners[GetEdgeEndVertex(CubeTable.Edges[triTableValue, 0])]) /
                    (cubeCorners[GetEdgeEndVertex(CubeTable.Edges[triTableValue, 1])] - cubeCorners[GetEdgeEndVertex(CubeTable.Edges[triTableValue, 0])]));

                vertices.Add(vertex);
                triangles.Add(vertices.Count - 1);

                edgeIndex++;
            }
        }
    }

    private int GetEdgeEndVertex(Vector3 pos)
    {
        for (int i = 0; i < CubeTable.Corners.Length; i++)
        {
            if (pos == CubeTable.Corners[i]) { return i; }
        }

        return default;
    }

    private int GetConfigurationIndex(float[] cubeCorners)
    {
        int configIndex = 0;

        // Starting byte
        // 0000 0000

        for (int i = 0; i < 8; i++)
        {
            // Will replace a single bit with a 1 if above threshhold
            if (cubeCorners[i] > heightThreshhold)
            {
                configIndex |= 1 << i;
            }
        }

        return configIndex;
    }

    /// <summary>
    /// Sets mesh of cube
    /// </summary>
    private void SetMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (!visualizeNoise || !Application.isPlaying) { return; }
        for (int row = 0; row < heights.GetLength(0); row++)
        {
            for (int col = 0; col < heights.GetLength(1); col++)
            {
                for (int x = 0; x < heights.GetLength(2); x++)
                {
                    Gizmos.color = new Color(heights[row, col, x], heights[row, col, x], heights[row, col, x], 1);
                    Gizmos.DrawSphere(new Vector3(row, col, x), .2f);
                }
            }
        }
    }
}
