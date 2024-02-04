using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class WorldArray : MonoBehaviour
{
    [SerializeField] private Vector2Int ArrayDimensions;
    [SerializeField] private float scale;

    private MeshFilter meshFilter;
    private MeshRenderer meshRend;

    private FastNoiseLite noise;
    private Vertex[,] mapFloor;

    private List<Triangle> triangles;
    private int triangleIndex;

    private Vector2 mousePos;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        this.noise = new FastNoiseLite();
        this.noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        this.noise.SetSeed(0);
        this.noise.SetFrequency(.2f);

        this.triangles = new List<Triangle>();

        this.mapFloor = new Vertex[ArrayDimensions.x + 1, ArrayDimensions.y + 1];
        CreateArray();
        TriangulatePoints();
        SetMesh();
    }

    public void MousePress(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed())
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        


    }

    private void CreateArray()
    {
        int width = ArrayDimensions.x;
        int height = ArrayDimensions.y;

        Vector2 offset = new Vector2(-(width * scale) / 2, -(height * scale) / 2);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                mapFloor[i, j] = new Vertex(new Vector2(i * scale, j * scale) + offset, 0 /* noise.GetNoise(i, j)*/);
            }
        }
    }

    private void TriangulatePoints()
    {
        int width = ArrayDimensions.x - 1;
        int height = ArrayDimensions.y - 1;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                TriangulatePoint(i, j, mapFloor[i, j]);
            }
        }
    }

    private void TriangulatePoint(int i, int j, Vertex v)
    {
        Vector3 topLeft = mapFloor[i, j].Position;
        Vector3 topRight = mapFloor[i + 1, j].Position;
        Vector3 bottomLeft = mapFloor[i, j + 1].Position;
        Vector3 bottomRight = mapFloor[i + 1, j + 1].Position;

        int point1Data = Mathf.CeilToInt(mapFloor[i, j].Data);
        int point2Data = Mathf.CeilToInt(mapFloor[i + 1, j].Data);
        int point3Data = Mathf.CeilToInt(mapFloor[i + 1, j + 1].Data);
        int point4Data = Mathf.CeilToInt(mapFloor[i, j + 1].Data);

        int state = GetState(point1Data, point2Data, point3Data, point4Data);

        Vector3 centerUp = Vector3.Lerp(topLeft, topRight, .5f);
        Vector3 centerRight = Vector3.Lerp(topRight, bottomRight, .5f);
        Vector3 centerDown = Vector3.Lerp(bottomRight, bottomLeft, .5f);
        Vector3 centerLeft = Vector3.Lerp(topLeft, bottomLeft, .5f);

        #region Switch
        switch (state)
        {
            case 0:
                break;

            // 1 Point
            case 1:
                triangles.Add(new Triangle(bottomLeft, centerLeft, centerDown, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 2:
                triangles.Add(new Triangle(bottomRight, centerDown, centerRight, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 4:
                triangles.Add(new Triangle(topRight, centerRight, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 8:
                triangles.Add(new Triangle(centerUp, centerLeft, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            // 2 Point          

            case 3:
                triangles.Add(new Triangle(bottomRight, bottomLeft, centerRight, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomLeft, centerLeft, centerRight, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 6:
                triangles.Add(new Triangle(topRight, bottomRight, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomRight, centerDown, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 9:
                triangles.Add(new Triangle(centerDown, bottomLeft, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerUp, centerDown, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 12:
                triangles.Add(new Triangle(topRight, centerRight, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerRight, centerLeft, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 5:
                triangles.Add(new Triangle(topRight, centerRight, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerRight, centerDown, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerDown, bottomLeft, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomLeft, centerLeft, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 10:
                triangles.Add(new Triangle(centerUp, centerRight, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerRight, bottomRight, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomRight, centerDown, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerDown, centerLeft, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            // 3 Point

            case 7:
                triangles.Add(new Triangle(topRight, bottomRight, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomRight, bottomLeft, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomLeft, centerLeft, centerUp, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 11:
                triangles.Add(new Triangle(centerUp, centerRight, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerRight, bottomRight, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomRight, bottomLeft, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 13:
                triangles.Add(new Triangle(topRight, centerRight, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerRight, centerDown, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerDown, bottomLeft, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 14:
                triangles.Add(new Triangle(topRight, bottomRight, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomRight, centerDown, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(centerDown, centerLeft, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                break;

            case 15:
                triangles.Add(new Triangle(topRight, bottomRight, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                triangles.Add(new Triangle(bottomRight, bottomLeft, topLeft, triangleIndex++, triangleIndex++, triangleIndex++));
                break;
                #endregion
        }
    }

    private void SetMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triIndices = new List<int>();

        foreach (Triangle tri in triangles)
        {
            vertices.AddRange(tri.GetPoints());
            triIndices.AddRange(tri.GetIndices());
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triIndices.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    ///// <summary>
    ///// The lines we will use for the triangle positions
    ///// </summary>
    //private void CreateLineList()
    //{
    //    float lerpAmt = .5f;
    //    Vector2 a = new Vector2(Mathf.Lerp(0, scale, lerpAmt), 0); // top edge
    //    Vector2 b = new Vector2(scale, Mathf.Lerp(0, scale, lerpAmt));   // right edge
    //    Vector2 c = new Vector2(Mathf.Lerp(0, scale, lerpAmt), scale);  // bottom edge
    //    Vector2 d = new Vector2(0, Mathf.Lerp(0, scale, lerpAmt));  // left edge

    //    midPoints = new Dictionary<string, Vector2>()
    //        {
    //            { "a", a },
    //            { "b", b },
    //            { "c", c },
    //            { "d", d },
    //        };
    //}

    /// <summary>
    /// Gets the state of the 4 vertices represented by a byte
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    private int GetState(int a, int b, int c, int d)
    {
        return a * 8 + b * 4 + c * 2 + d * 1;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Vertex v in mapFloor)
        {
            Gizmos.color = Color.black + Color.white * v.Data;
            Gizmos.DrawCube(v.Position, Vector3.one / 5);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos, .2f);
    }
}
