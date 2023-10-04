using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomGen : MonoBehaviour
{
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject roomTile;
    [SerializeField] private GameObject doorTile;

    [SerializeField] private Vector2 pos;

    private Vector3 tileScale;
    private GameObject[,] roomGrid;

    public bool IsGenerated { get; private set; }
    public float xMin { get; private set; }
    public float yMin { get; private set; }
    public float xMax { get; private set; }
    public float yMax { get; private set; }

    // Start is called before the first frame update
    public void GenerateRoomGrid(float x, float y, int width, int height)
    {
        pos = new Vector2(x, y);

        roomGrid = new GameObject[width, height];
        tileScale = wallTile.transform.localScale;

        for (int row = 0; row < width; row++)
        {
            for (int col = 0; col < height; col++)
            {
                Vector3 position = new Vector3(x + (tileScale.x * .5f) - (tileScale.x * width / 2) + (row * tileScale.x), y + (tileScale.y * .5f) - (tileScale.y * height / 2) + (col * tileScale.y));

                if (row == 0 || row == width - 1 || col == 0 || col == height - 1)
                {
                    roomGrid[row, col] = Instantiate(wallTile, position + new Vector3(0,0,2), Quaternion.identity);
                }
                else
                {
                    roomGrid[row, col] = Instantiate(roomTile, position, Quaternion.identity);
                }

                roomGrid[row, col].transform.parent = transform;
            }
        }

        xMin = x - width * tileScale.x / 2 + tileScale.x / 2;
        xMax = x + width * tileScale.x / 2 + tileScale.x / 2;
        yMin = y - height * tileScale.y / 2 + tileScale.y / 2;
        yMax = y + height * tileScale.y / 2 + tileScale.y / 2;

        IsGenerated = true;
    }

    public void AddDoorConnection(Vector3 teleportPos, Vector2 direction)
    {

    }
}
