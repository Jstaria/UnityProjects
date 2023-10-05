using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
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
    private Vector4[] possibleDoorLocations = new Vector4[4];

    public Vector2[] DoorTPLocations { get; private set; }
    public bool IsGenerated { get; private set; }
    public float xMin { get; private set; }
    public float yMin { get; private set; }
    public float xMax { get; private set; }
    public float yMax { get; private set; }

    // Start is called before the first frame update
    public void GenerateRoomGrid(float x, float y, int width, int height)
    {
        DoorTPLocations = new Vector2[4];

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

        DoorTPLocations[0] = new Vector2(x, yMax - 3 * tileScale.y); // top
        DoorTPLocations[1] = new Vector2(xMax - 3 * tileScale.x, y); // right
        DoorTPLocations[2] = new Vector2(x, yMin + 2 * tileScale.y); // bottom
        DoorTPLocations[3] = new Vector2(xMin + 2 * tileScale.x, y); // left

        possibleDoorLocations[0] = new Vector4(roomGrid.GetLength(0) / 2, roomGrid.GetLength(1) - 1,
                                               roomGrid.GetLength(0) / 2 - 1, roomGrid.GetLength(1) - 1); // top

        possibleDoorLocations[1] = new Vector4(roomGrid.GetLength(0) - 1, roomGrid.GetLength(1) / 2, 
                                               roomGrid.GetLength(0) - 1, roomGrid.GetLength(1) / 2 - 1); // right

        possibleDoorLocations[2] = new Vector4(roomGrid.GetLength(0) / 2, 0, roomGrid.GetLength(0) / 2 - 1, 0); // bottom

        possibleDoorLocations[3] = new Vector4(0, roomGrid.GetLength(1) / 2, 0, roomGrid.GetLength(1) / 2 - 1); // left
    }

    public void AddDoorConnection(Vector3 teleportPos, int direction)
    {
        int[] doorOne = new int[] { (int)possibleDoorLocations[direction].x, (int)possibleDoorLocations[direction].y };
        int[] doorTwo = new int[] { (int)possibleDoorLocations[direction].z, (int)possibleDoorLocations[direction].w };

        Vector2 position = roomGrid[doorOne[0], doorOne[1]].transform.position;

        GameObject.Destroy(roomGrid[doorOne[0], doorOne[1]]);
        roomGrid[doorOne[0], doorOne[1]] = Instantiate(doorTile, position, Quaternion.identity);
        roomGrid[doorOne[0], doorOne[1]].transform.parent = transform;
        roomGrid[doorOne[0], doorOne[1]].GetComponent<DoorBehavior>().CreateDoor(teleportPos);

        position = roomGrid[doorTwo[0], doorTwo[1]].transform.position;

        GameObject.Destroy(roomGrid[doorTwo[0], doorTwo[1]]);
        roomGrid[doorTwo[0], doorTwo[1]] = Instantiate(doorTile, position, Quaternion.identity);
        roomGrid[doorTwo[0], doorTwo[1]].transform.parent = transform;
        roomGrid[doorTwo[0], doorTwo[1]].GetComponent<DoorBehavior>().CreateDoor(teleportPos);
    }
}
