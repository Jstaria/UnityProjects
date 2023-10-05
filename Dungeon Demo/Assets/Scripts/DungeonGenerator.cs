using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField][Range(1,30)] private int numberOfRooms;
    [SerializeField] private GameObject tileSample;
    [SerializeField] private GameObject room;

    [SerializeField] private int spaceBetweenRooms;

    [SerializeField] private int minRoomWidth;
    [SerializeField] private int maxRoomWidth;
    [SerializeField] private int minRoomHeight;
    [SerializeField] private int maxRoomHeight;

    private GameObject[,] rooms;
    private Vector2 prevRoom;
    private int activeRooms = 0;
    private GameObject[,] roomFloors;

    public bool DoneGenerating { get; private set; }
    public GameObject[,] Rooms { get { return rooms; } }

    // Start is called before the first frame update
    void Start()
    {
        rooms = new GameObject[9, 9];
        roomFloors = new GameObject[rooms.GetLength(0), rooms.GetLength(1)];

        // Creates the grid and fills it with inactive rooms
        for (int i = 0; i < rooms.GetLength(0); i++)
        {
            for (int j = 0; j < rooms.GetLength(1); j++)
            {
                rooms[i, j] = Instantiate(room, new Vector3(i * spaceBetweenRooms - (spaceBetweenRooms * rooms.GetLength(0) / 2) + (spaceBetweenRooms / 2), j * spaceBetweenRooms - (spaceBetweenRooms * rooms.GetLength(1) / 2) + (spaceBetweenRooms / 2)), Quaternion.identity);
                rooms[i, j].transform.parent = transform;
                rooms[i, j].SetActive(false);

                roomFloors[i, j] = Instantiate(tileSample, new Vector3(i * spaceBetweenRooms - (spaceBetweenRooms * rooms.GetLength(0) / 2) + (spaceBetweenRooms / 2), j * spaceBetweenRooms - (spaceBetweenRooms * rooms.GetLength(1) / 2) + (spaceBetweenRooms / 2), 20), Quaternion.identity);
                roomFloors[i, j].transform.localScale = new Vector3(spaceBetweenRooms, spaceBetweenRooms);
                roomFloors[i, j].transform.parent = GameObject.Find("Floor").transform;
            }
        }

        while (activeRooms < numberOfRooms)
        {
            float x, y; 
            int wRan, hRan;
            
            // height and width can afford to be random with grid like layout since we can determine the distance between all of the rooms
            wRan = Random.Range(minRoomWidth / 2, maxRoomWidth / 2) * 2;
            hRan = Random.Range(minRoomHeight / 2, maxRoomHeight / 2) * 2;

            // Spawn room will always be center
            if (activeRooms == 0)
            {
                int gridSpotX = rooms.GetLength(0) / 2;
                int gridSpotY = rooms.GetLength(1) / 2;
                Vector2 roomPos = rooms[gridSpotX, gridSpotY].transform.position;

                x = roomPos.x;
                y = roomPos.y;

                // Activating room and generating its own grid
                rooms[gridSpotX, gridSpotY].GetComponent<RoomGen>().GenerateRoomGrid(x, y, wRan, hRan);
                rooms[gridSpotX, gridSpotY].SetActive(true);

                activeRooms++;
                prevRoom = new Vector2(gridSpotX, gridSpotY);
            }

            // Otherwise, it will snake from center
            else
            {
                // Finds suitable neighbor
                Vector2 currentRoom = FindSuitableNeighbor(prevRoom);

                // If in that function, it determines there is no other way to generate the map, it will set the roomNum to the active rooms and it will quite generating
                if (activeRooms == numberOfRooms) { continue; }

                Vector2 roomPos = rooms[(int)currentRoom.x, (int)currentRoom.y].transform.position;

                x = roomPos.x;
                y = roomPos.y;

                rooms[(int)currentRoom.x, (int)currentRoom.y].GetComponent<RoomGen>().GenerateRoomGrid(x, y, wRan, hRan);
                rooms[(int)currentRoom.x, (int)currentRoom.y].transform.parent = transform;
                rooms[(int)currentRoom.x, (int)currentRoom.y].SetActive(true);

                activeRooms++;
            }
        }

        Vector2[] directions = new Vector2[]
        {
            new Vector2(0, 1), // Up
            new Vector2(1, 0), // Right
            new Vector2(0, -1), // Down
            new Vector2(-1, 0), // Left
        };

        for (int i = 0; i < rooms.GetLength(0); i++)
        {
            for (int j = 0; j < rooms.GetLength(1); j++)
            {
                if (!rooms[i, j].GetComponent<RoomGen>().IsGenerated) { continue; }

                for (int k = 0; k < 4; k++)
                {
                    int[] nextGridCoords = new int[2] { i + (int)directions[k].x, j + (int)directions[k].y };

                    if (nextGridCoords[0] < rooms.GetLength(0) && nextGridCoords[1] < rooms.GetLength(1) &&
                    nextGridCoords[0] >= 0 && nextGridCoords[1] >= 0)
                    {
                        if (!rooms[nextGridCoords[0], nextGridCoords[1]].GetComponent<RoomGen>().IsGenerated) { continue; }

                        Vector2 tpPos = rooms[nextGridCoords[0], nextGridCoords[1]].GetComponent<RoomGen>().DoorTPLocations[(k + 2) % 4];
                        rooms[i, j].GetComponent<RoomGen>().AddDoorConnection(tpPos, k);
                    }
                }
            }
        }

       DoneGenerating = true;
    }

    private Vector2 FindSuitableNeighbor(Vector2 prevRoom)
    {
        Vector2 nextGridCoords = Vector2.zero;

        Vector2[] directions = new Vector2[]
        {
            new Vector2(0, 1), // Up
            new Vector2(1, 0), // Right
            new Vector2(0, -1), // Down
            new Vector2(-1, 0), // Left
        };

        Vector2 ranDir = Vector2.zero;

        int stuck = 0;
        bool restartFromCenter = false;

        do
        {
            int stuckDir = 0;
            do
            {
                ranDir = directions[Random.Range(0, 4)];
                nextGridCoords = ranDir + prevRoom;

                if (stuckDir > 4)
                {
                    nextGridCoords = new Vector2(rooms.GetLength(0) / 2, rooms.GetLength(1) / 2);
                }

                stuckDir++;
            }
            while (nextGridCoords.x >= rooms.GetLength(0) || nextGridCoords.y >= rooms.GetLength(1) ||
                   nextGridCoords.x < 0 || nextGridCoords.y < 0);

            if (stuck > 10) { numberOfRooms = activeRooms; break; }

            stuck++;
        }
        while (rooms[(int)(nextGridCoords.x), (int)(nextGridCoords.y)].activeSelf || restartFromCenter);

        this.prevRoom = nextGridCoords;

        if (restartFromCenter)
        {
            int gridSpotX = rooms.GetLength(0) / 2;
            int gridSpotY = rooms.GetLength(1) / 2;
            Vector2 roomPos = rooms[gridSpotX, gridSpotY].transform.position;

            float x = roomPos.x;
            float y = roomPos.y;

            int wRan = Random.Range(minRoomWidth / 2, maxRoomWidth / 2) * 2;
            int hRan = Random.Range(minRoomHeight / 2, maxRoomHeight / 2) * 2;

            GameObject.Destroy(rooms[gridSpotX, gridSpotY]);

            rooms[gridSpotX, gridSpotY] = Instantiate(room, roomPos, Quaternion.identity);

            // Activating room and generating its own grid
            rooms[gridSpotX, gridSpotY].GetComponent<RoomGen>().GenerateRoomGrid(x, y, wRan, hRan);
            rooms[gridSpotX, gridSpotY].SetActive(true);
        }

        return nextGridCoords;
    }
}
