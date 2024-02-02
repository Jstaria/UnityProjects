using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{
    private ChunkData chunk;
    private FastNoiseLite noise;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private List<Vector3> vertices;
    private int[] triangles;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int depth;

    [SerializeField] private int scale;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        //meshCollider = GetComponent<MeshCollider>();

        noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFrequency(.3f);
        noise.SetSeed(0);
        chunk = new ChunkData(width, height, depth, Vector3Int.zero, scale, noise);
        SetMesh();
    }

    private void SetMesh()
    {
        Mesh mesh = new Mesh();

        vertices = new List<Vector3>();

        List<Vector3[]> meshVertices = chunk.Triangles;

        foreach (Vector3[] va in meshVertices)
        {
            foreach (Vector3 v in va)
            {
                vertices.Add(v);
            }
        }

        triangles = new int[vertices.Count];

        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        //meshCollider.sharedMesh = mesh;
    }
}
