using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkData
{
    private Vector3[][,] meshVertices;
    private float[][,] meshVerticeData;
    private Vector3Int worldPosition;

    private FastNoiseLite noise;

    private int widthX, heightY, lengthZ, scale;

    private List<Vector3Int> directions;
    private Dictionary<string, Vector2> midPoints;
    private List<Vector3[]> triangles;

    public Vector3[][,] MeshVertices { get { return meshVertices; } }
    public float[][,] MeshVerticeData { get { return meshVerticeData; } }
    public List<Vector3[]> Triangles { get { return triangles; } }

    public ChunkData(int widthX, int heightY, int lengthZ, Vector3Int worldPosition, int scale, FastNoiseLite noise)
    {
        this.widthX = widthX;
        this.heightY = heightY;
        this.lengthZ = lengthZ;
        this.scale = scale;

        this.worldPosition = worldPosition;

        this.noise = noise;

        CreateLineList();
        SetDirections();
        SetFirstChunk(widthX, heightY, lengthZ, worldPosition);
    }

    private void SetFirstChunk(int widthX, int heightY, int lengthZ, Vector3Int worldPosition)
    {
        meshVertices = new Vector3[6][,];
        meshVerticeData = new float[6][,];

        CreateVertices(widthX, heightY, lengthZ, worldPosition);
        //DoubleFaceVertices(widthX, heightY, lengthZ);
    }

    private void CreateVertices(int widthX, int heightY, int lengthZ, Vector3Int worldPosition)
    {
        //for (int i = 0; i < widthX + 1; i++)
        //{
        //    for (int j = 0; j < heightY + 1; j++)
        //    {
        //        for (int k = 0; k < lengthZ + 1; k++)
        //        {
        //            meshVertices[i, j, k] = worldPosition + new Vector3(i * scale, j * scale, k * scale);

        //            if (i == 0 || i == widthX || j == 0 || j == heightY || k == 0 || k == lengthZ)
        //            {
        //                meshVerticeData[i, j, k] = noise.GetNoise(i + worldPosition.x, j + worldPosition.y, k + worldPosition.z);
        //            }
        //            else
        //            {
        //                meshVerticeData[i, j, k] = -1f;
        //            }
        //        }
        //    }
        //}

        meshVertices[0] = new Vector3[widthX + 1, lengthZ + 1]; meshVerticeData[0] = new float[widthX + 1, lengthZ + 1]; // top
        meshVertices[1] = new Vector3[heightY + 1, widthX + 1]; meshVerticeData[1] = new float[heightY + 1, widthX + 1]; // front
        meshVertices[2] = new Vector3[widthX + 1, lengthZ + 1]; meshVerticeData[2] = new float[widthX + 1, lengthZ + 1]; // bottom
        meshVertices[3] = new Vector3[heightY + 1, widthX + 1]; meshVerticeData[3] = new float[heightY + 1, widthX + 1]; // back
        meshVertices[4] = new Vector3[heightY + 1, lengthZ + 1]; meshVerticeData[4] = new float[heightY + 1, lengthZ + 1]; // left
        meshVertices[5] = new Vector3[heightY + 1, lengthZ + 1]; meshVerticeData[5] = new float[heightY + 1, lengthZ + 1]; // right

        triangles = new List<Vector3[]>();

        for (int i = 0; i < meshVertices.Length; i++)
        {
            for (int j = 0; j < meshVertices[i].GetLength(0); j++)
            {
                for (int k = 0; k < meshVertices[i].GetLength(1); k++)
                {
                    switch(i)
                    {
                        case 0:
                            meshVertices[i][j, k] = new Vector3(j * scale, (heightY + 1) * scale, k * scale) + worldPosition; // top
                            meshVerticeData[i][j, k] = noise.GetNoise(j + worldPosition.x, (heightY + 1) + worldPosition.y, k + worldPosition.z);
                            break;
                        case 1:
                            meshVertices[i][j, k] = new Vector3(k * scale, j * scale, (lengthZ + 1) * scale) + worldPosition; // front
                            meshVerticeData[i][j, k] = noise.GetNoise(j + worldPosition.x, k + worldPosition.y, (lengthZ + 1) + worldPosition.z);
                            break;
                        case 2:
                            meshVertices[i][j, k] = new Vector3(j * scale, 0, k * scale) + worldPosition; // bottom
                            meshVerticeData[i][j, k] = noise.GetNoise(j + worldPosition.x, worldPosition.y, k + worldPosition.z);
                            break;
                        case 3:
                            meshVertices[i][j, k] = new Vector3(k * scale, j * scale, 0) + worldPosition; // back
                            meshVerticeData[i][j, k] = noise.GetNoise(j + worldPosition.x, k + worldPosition.y, worldPosition.z);
                            break;
                        case 4:
                            meshVertices[i][j, k] = new Vector3(0, j * scale, k * scale) + worldPosition; // left
                            meshVerticeData[i][j, k] = noise.GetNoise(worldPosition.x, j + worldPosition.y, k + worldPosition.z);
                            break;
                        case 5:
                            meshVertices[i][j, k] = new Vector3((widthX + 1) * scale , j * scale, k * scale) + worldPosition; // right
                            meshVerticeData[i][j, k] = noise.GetNoise((widthX + 1) + worldPosition.x, j + worldPosition.y, k + worldPosition.z);
                            break;
                    }
                }
            }
        }

        TriangulateAllPoints();
    }

    private void TriangulateAllPoints()
    {
        for (int i = 0; i < meshVertices.Length; i++)
        {
            for (int j = 0; j < meshVertices[i].GetLength(0) - 1; j++)
            {
                for (int k = 0; k < meshVertices[i].GetLength(1) - 1; k++)
                {
                    TriangulatePoint(i, j, k);
                }
            }
        }
    }

    private void TriangulatePoint(int i, int j, int k)
    {
        Vector3 topLeft = meshVertices[i][j, k];
        Vector3 topRight = meshVertices[i][j + 1, k];
        Vector3 bottomLeft = meshVertices[i][j, k + 1];
        Vector3 bottomRight = meshVertices[i][j + 1, k + 1];

        int point1Data = Mathf.CeilToInt(meshVerticeData[i][j, k]);
        int point2Data = Mathf.CeilToInt(meshVerticeData[i][j + 1, k]);
        int point4Data = Mathf.CeilToInt(meshVerticeData[i][j, k + 1]);
        int point3Data = Mathf.CeilToInt(meshVerticeData[i][j + 1, k + 1]);

        int state = GetState(point1Data, point2Data, point3Data, point4Data);

        Vector3 centerUp = Vector3.Lerp(topLeft, topRight, .5f);
        Vector3 centerRight = Vector3.Lerp(topRight, bottomRight, .5f);
        Vector3 centerDown = Vector3.Lerp(bottomRight, bottomLeft, .5f);
        Vector3 centerLeft = Vector3.Lerp(topLeft, bottomLeft, .5f);

        switch (state)
        {
            case 0:
                break;

            // 1 Point
            case 1:
                triangles.Add(new Vector3[] { centerDown, bottomLeft, centerLeft });
                break;

            case 2:
                triangles.Add(new Vector3[] { centerRight, bottomRight, centerDown });
                break;

            case 4:
                triangles.Add(new Vector3[] { centerUp, topRight, centerRight });
                break;

            case 8:
                triangles.Add(new Vector3[] { topLeft, centerUp, centerLeft });
                break;

            // 2 Point

            case 3:
                triangles.Add(new Vector3[] { centerRight, bottomRight, bottomLeft });
                triangles.Add(new Vector3[] { centerRight, bottomLeft, centerLeft });
                break;

            case 6:
                triangles.Add(new Vector3[] { centerUp, topRight, bottomRight });
                triangles.Add(new Vector3[] { centerUp, bottomRight, centerDown });
                break;

            case 9:
                triangles.Add(new Vector3[] { topLeft, centerDown, bottomLeft });
                triangles.Add(new Vector3[] { topLeft, centerUp, centerDown });
                break;

            case 12:
                triangles.Add(new Vector3[] { topLeft, topRight, centerRight });
                triangles.Add(new Vector3[] { topLeft, centerRight, centerLeft });
                break;

            case 5:
                triangles.Add(new Vector3[] { centerUp, topRight, centerRight });
                triangles.Add(new Vector3[] { centerUp, centerRight, centerDown });
                triangles.Add(new Vector3[] { centerUp, centerDown, bottomLeft });
                triangles.Add(new Vector3[] { centerUp, bottomLeft, centerLeft });
                break;

            case 10:
                triangles.Add(new Vector3[] { topLeft, centerUp, centerRight });
                triangles.Add(new Vector3[] { topLeft, centerRight, bottomRight });
                triangles.Add(new Vector3[] { topLeft, bottomRight, centerDown });
                triangles.Add(new Vector3[] { topLeft, centerDown, centerLeft });
                break;

            // 3 Point

            case 7:
                triangles.Add(new Vector3[] { centerUp, topRight, bottomRight });
                triangles.Add(new Vector3[] { centerUp, bottomRight, bottomLeft });
                triangles.Add(new Vector3[] { centerUp, bottomLeft, centerLeft });
                break;

            case 11:
                triangles.Add(new Vector3[] { topLeft, centerUp, centerRight });
                triangles.Add(new Vector3[] { topLeft, centerRight, bottomRight });
                triangles.Add(new Vector3[] { topLeft, bottomRight, bottomLeft });
                break;

            case 13:
                triangles.Add(new Vector3[] { topLeft, topRight, centerRight });
                triangles.Add(new Vector3[] { topLeft, centerRight, centerDown });
                triangles.Add(new Vector3[] { topLeft, centerDown, bottomLeft });
                break;

            case 14:
                triangles.Add(new Vector3[] { topLeft, topRight, bottomRight });
                triangles.Add(new Vector3[] { topLeft, bottomRight, centerDown });
                triangles.Add(new Vector3[] { topLeft, centerDown, centerLeft });
                break;

            case 15:
                triangles.Add(new Vector3[] { topLeft, topRight, bottomRight });
                triangles.Add(new Vector3[] { topLeft, bottomRight, bottomLeft });
                break;
        }
    }

    #region NotInUse

    private void DoubleFaceVertices(int widthX, int heightY, int lengthZ)
    {
        foreach (Vector3Int face in directions)
        {
            SetFace(face, widthX, heightY, lengthZ);
        }
    }

    private void SetFace(Vector3Int face, int widthX, int heightY, int lengthZ)
    {
        Vector3Int faceVertices = new Vector3Int(widthX, heightY, lengthZ) * face;

        for (int i = 1; i < faceVertices.x; i++)
        {
            for (int j = 1; j < faceVertices.x; j++)
            {
                for (int k = 1; k < faceVertices.x; k++)
                {
                    //meshVerticeData[i, j, k] = meshVerticeData[i - face.x, j - face.y, k - face.z];
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// The lines we will use for the triangle positions
    /// </summary>
    private void CreateLineList()
    {
        float lerpAmt = .5f;
        Vector2 a = new Vector2(Mathf.Lerp(0, scale, lerpAmt), 0); // top edge
        Vector2 b = new Vector2(scale, Mathf.Lerp(0, scale, lerpAmt));   // right edge
        Vector2 c = new Vector2(Mathf.Lerp(0, scale, lerpAmt), scale);  // bottom edge
        Vector2 d = new Vector2(0, Mathf.Lerp(0, scale, lerpAmt));  // left edge

        midPoints = new Dictionary<string, Vector2>()
            {
                { "a", a },
                { "b", b },
                { "c", c },
                { "d", d },
            };
    }

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

    private void SetDirections()
    {
        directions = new List<Vector3Int>()
        {
            new Vector3Int(0, 1, 1),
            new Vector3Int(1, 0, 1),
            new Vector3Int(1, 1, 0),
            new Vector3Int(0, -1, -1),
            new Vector3Int(-1, 0, -1),
            new Vector3Int(-1, -1, 0),
        };
    }
}
