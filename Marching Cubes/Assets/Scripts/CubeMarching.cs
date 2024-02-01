using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]

public class CubeMarching : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int length = 10;
    [SerializeField] private int height = 10;

    [SerializeField] private float noiseResolution = 1;
    [SerializeField] private bool visualizeNoise = true;

    [SerializeField] private float heightThreshhold = .5f;

    private float[,,] heights;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        StartCoroutine(UpdateAll());

    }

    #region Generic

    private IEnumerator UpdateAll()
    {
        while (true)
        {
            SetHeights();
            MarchCubes();
            SetMesh();
            yield return new WaitForSeconds(1);
        }
    }

    private void MarchCubes()
    {
        vertices.Clear();
        triangles.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < length; z++)
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
        heights = new float[width + 1, height + 1, length + 1];

        // Loop through each height vertex
        for (int x  = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height + 1; y++)
            {
                for (int z = 0; z < length + 1; z++)
                {
                    // 2D Noise
                    // Using perlin noise to get a height value
                    float currentHeight = Mathf.Pow(Mathf.PerlinNoise(x * noiseResolution, z * noiseResolution) * Mathf.PerlinNoise(Mathf.Pow(y, noiseResolution), x * noiseResolution), noiseResolution) * height;

                    // Checking the depth value and assigning it(I'm pretty sure)
                    float newHeight = y <= currentHeight - 0.5f ? 0f : 
                                      y > currentHeight + 0.5f ? 1f :
                                      y > currentHeight ? y - currentHeight : 
                                      currentHeight - y;

                    heights[x, y, z] = newHeight;

                    // 3D Noise
                    //float currentHeight = (float)NoiseS3D.Noise(x * noiseResolution / 15, y * noiseResolution / 15, z * noiseResolution / 15) / Mathf.PerlinNoise(x * noiseResolution, z * noiseResolution);
                    //heights[x, y, z] = currentHeight;
                }
            }
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (!visualizeNoise || !Application.isPlaying) { return; }

        for (int x = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height + 1; y++)
            {
                for (int z = 0; z < length + 1; z++)
                {
                    Gizmos.color = new Color(heights[x, y, z], heights[x, y, z], heights[x, y, z], 1);
                    Gizmos.DrawSphere(new Vector3(x, y, z), .2f);
                }
            }
        }
    }
}
