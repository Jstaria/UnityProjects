using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]

public class GameTileCreation : MonoBehaviour
{
    private int width ;
    private int length;
    private int height;

    [SerializeField] private bool visualizeNoise = true;

    [SerializeField] private float heightThreshhold = .5f;

    private float[,,] heights;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private Camera cam;

    private List<List<string>> tileData = new List<List<string>>();

    // Start is called before the first frame update
    void Start()
    {
        List<string> tileData = FileIO.ReadFrom("Test");
        string[] data = tileData[0].Split(',');

        width = int.Parse(data[0]);
        height = int.Parse(data[1]);
        length = int.Parse(data[2]);

        cam = Camera.main;
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

    private void MarchCubes()
    {
        vertices.Clear();
        triangles.Clear();

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
                        cubeCorners[i] = heights[corner.x, corner.y, corner.z];
                    }

                    MarchCube(new Vector3(x, y, z), cubeCorners);
                }
            }
        }
    }

    private void MarchCube(Vector3 position, float[] cubeCorners)
    {
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

    private void SetHeights()
    {
        heights = new float[width, height, length];

        int i = 1;
        int count = 0;
        List<string> tileData = FileIO.ReadFrom("Test");
        string[] data;
        List<string> temp = new List<string>();

        for (int j = 0; j < height + 1; j++)
        {
            this.tileData.Add(new List<string>());
        }

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

        // Loop through each height vertex
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                data = this.tileData[y][x].Split(',');

                for (int z = 0; z < length; z++)
                {
                    heights[x, y, z] = float.Parse(data[z]);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!visualizeNoise || !Application.isPlaying) { return; }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < length; z++)
                {
                    Gizmos.color = new Color(heights[x, y, z], heights[x, y, z], heights[x, y, z], 1);
                    Gizmos.DrawSphere(new Vector3(x, y, z), .2f);
                }
            }
        }
    }
}
