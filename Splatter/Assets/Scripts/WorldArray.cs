using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldArray : MonoBehaviour
{
    [SerializeField] private Vector2Int ArrayDimensions;
    [SerializeField] private float scale;

    private FastNoiseLite noise;
    private Vertex[,] mapFloor;

    private void Start()
    {
        noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetSeed(0);
        noise.SetFrequency(.2f);

        mapFloor = new Vertex[ArrayDimensions.x + 1, ArrayDimensions.y + 1];
        CreateArray();
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
                mapFloor[i,j] = new Vertex(new Vector2(i * scale, j * scale) + offset, noise.GetNoise(i,j));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Vertex v in mapFloor)
        {
            Gizmos.color = Color.black + Color.white * v.Data;
            Gizmos.DrawCube(v.Position, Vector3.one / 5);
        }
    }
}
