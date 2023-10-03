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

    private Vector3 tileScale;

    private GameObject[,] roomGrid;

    public int[] AvailableDoorSpots { get; private set; }
    public float xMin { get; private set; }
    public float yMin { get; private set; }
    public float xMax { get; private set; }
    public float yMax { get; private set; }

    // Start is called before the first frame update
    public void GenerateRoomGrid(int x, int y, int width, int height, int[] availableDoorSpots)
    {
        AvailableDoorSpots = availableDoorSpots;
        roomGrid = new GameObject[width, height];
        tileScale = wallTile.transform.localScale;

        for (int row = 0; row < width; row++)
        {
            for (int col = 0; col < height; col++)
            {
                Vector3 position = new Vector3((tileScale.x * .5f) - (tileScale.x * width / 2) + (row * tileScale.x), (tileScale.y * .5f) - (tileScale.y * height / 2) + (col * tileScale.y));

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

        //AvailableDoorSpots = new int[] { 1, 1, 1, 1 };

        // Left
        if (AvailableDoorSpots[0] == 1)
        {
            Transform tile = roomGrid[0, height / 2].transform;
            GameObject.Destroy(roomGrid[0, height / 2]);
            roomGrid[0, height / 2] = Instantiate(doorTile, tile.position, Quaternion.identity);

            tile = roomGrid[0, height / 2 - 1].transform;
            GameObject.Destroy(roomGrid[0, height / 2 - 1]);
            roomGrid[0, height / 2 - 1] = Instantiate(doorTile, tile.position, Quaternion.identity);
        }

        // Top
        if (AvailableDoorSpots[1] == 1)
        {
            Transform tile = roomGrid[width / 2, 0].transform;
            GameObject.Destroy(roomGrid[width / 2, 0]);
            roomGrid[width / 2, 0] = Instantiate(doorTile, tile.position, Quaternion.identity);

            tile = roomGrid[width / 2 - 1, 0].transform;
            GameObject.Destroy(roomGrid[width / 2 - 1, 0]);
            roomGrid[width / 2 - 1, 0] = Instantiate(doorTile, tile.position, Quaternion.identity);
        }

        // Right
        if (AvailableDoorSpots[2] == 1)
        {
            Transform tile = roomGrid[width - 1, height / 2].transform;
            GameObject.Destroy(roomGrid[width - 1, height / 2]);
            roomGrid[width - 1, height / 2] = Instantiate(doorTile, tile.position, Quaternion.identity);

            tile = roomGrid[width - 1, height / 2 - 1].transform;
            GameObject.Destroy(roomGrid[width - 1, height / 2 - 1]);
            roomGrid[width - 1, height / 2 - 1] = Instantiate(doorTile, tile.position, Quaternion.identity);
        }

        // Bottom
        if (AvailableDoorSpots[3] == 1)
        {
            Transform tile = roomGrid[width / 2, height - 1].transform;
            GameObject.Destroy(roomGrid[width / 2, height - 1]);
            roomGrid[width / 2, height - 1] = Instantiate(doorTile, tile.position, Quaternion.identity);

            tile = roomGrid[width / 2 - 1, height - 1].transform;
            GameObject.Destroy(roomGrid[width / 2 - 1, height - 1]);
            roomGrid[width / 2 - 1, height - 1] = Instantiate(doorTile, tile.position, Quaternion.identity);
        }

        xMin = x - width * tileScale.x / 2 + tileScale.x / 2;
        xMax = x + width * tileScale.x / 2 + tileScale.x / 2;
        yMin = y - height * tileScale.y / 2 + tileScale.y / 2;
        yMax = y + height * tileScale.y / 2 + tileScale.y / 2;
    }
}
