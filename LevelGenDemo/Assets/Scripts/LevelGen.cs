using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [SerializeField] private GameObject roomPoint;
    [SerializeField] private GameObject mapFloor;
    [SerializeField] private GameObject roomFloor;

    [Header("Floor")]
    [SerializeField][Range(1, 100)] private int floorHeight;
    [SerializeField][Range(1, 100)] private int floorWidth;
    [SerializeField][Range(1, 100)] private int numberOfRooms = 5;

    [Header("Room")]
    [SerializeField][Range(5, 15)] private int maxRoomWidth = 10;
    [SerializeField][Range(5, 15)] private int maxRoomHeight = 10;
    [SerializeField][Range(5, 10)] private int minRoomWidth = 10;
    [SerializeField][Range(5, 10)] private int minRoomHeight = 10;

    // The minimum space between room spawns
    [SerializeField] private float minDistance;

    [SerializeField] private bool ToggleRegeneration = false;

    private int roomSpawnTries = 0;
    private int trueNumberRooms;
    private List<GameObject> roomPoints = new List<GameObject>();
    private List<GameObject> rooms = new List<GameObject>();
    private Transform parentContainer;

    private void Start()
    {
        trueNumberRooms = numberOfRooms;
        parentContainer = GameObject.Find("FloorGen").transform;
        mapFloor = Instantiate(mapFloor, new Vector3(0, 0, 10), Quaternion.identity);
        mapFloor.transform.parent = parentContainer;
    }

    // Start is called before the first frame update
    void Update()
    {
        if (ToggleRegeneration)
        {
            SetupRoomLayout();
            SpawnRooms();
        }
        else
        {
            numberOfRooms = trueNumberRooms;

            for (int i = 0; i < roomPoints.Count; i++)
            {
                GameObject.Destroy(roomPoints[i]);
                GameObject.Destroy(rooms[i]);
            }

            roomPoints.Clear();
            rooms.Clear();
        }
    }

    private void SetupRoomLayout()
    {
        mapFloor.transform.localScale = new Vector3(-floorWidth, -floorHeight, 0);

        // So it doesn't always have to run all of this code
        if (roomPoints.Count == numberOfRooms) { return; }

        // First room location will always be 0,0
        if (roomPoints.Count == 0)
        {
            roomPoints.Add(Instantiate(roomPoint, Vector3.zero, Quaternion.identity));
            roomPoints[0].transform.parent = parentContainer;
        }

        // Generates up to number of rooms allowed
        if (roomPoints.Count < numberOfRooms)
        {
            int ranX = Random.Range(-floorWidth / 2, floorWidth / 2);
            int ranY = Random.Range(-floorHeight / 2, floorHeight / 2);

            if (CheckDistance(ranX, ranY))
            {
                roomPoints.Add(Instantiate(roomPoint, new Vector3(ranX, ranY, 0), Quaternion.identity));
                roomPoints[roomPoints.Count - 1].transform.parent = parentContainer;
            }
            else { roomSpawnTries++; }

            if (roomSpawnTries > 300) { numberOfRooms = roomPoints.Count; roomSpawnTries = 0; }
        }

        // Destroys any excess rooms, mainly for visual/debugging purposes, pretty to look at
        else if (roomPoints.Count > numberOfRooms)
        {
            GameObject.Destroy(roomPoints[roomPoints.Count - 1]);
            roomPoints.RemoveAt(roomPoints.Count - 1);
            GameObject.Destroy(rooms[rooms.Count - 1]);
            rooms.RemoveAt(rooms.Count - 1);
        }
    }

    /// <summary>
    /// Spacing check method
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckDistance(int x, int y)
    {
        for (int j = 0; j < roomPoints.Count; j++)
        {
            if ((roomPoints[j].transform.position - new Vector3(x, y, 0)).sqrMagnitude < Mathf.Pow(minDistance, 2))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Stand in random room sizes for hand crafted rooms
    /// </summary>
    private void SpawnRooms()
    {
        if (roomPoints.Count != numberOfRooms) { return; }
        if (rooms.Count == numberOfRooms) { return; }

        if (rooms.Count != roomPoints.Count)
        {
            int ranWidth = Random.Range(minRoomWidth, maxRoomWidth + 1);
            int ranHeight = Random.Range(minRoomHeight, maxRoomHeight + 1);

            if (rooms.Count != 0)
            {
                int stuckCount = 0;

                // I dont think check for overlap is currently workin gas I intended
                while (CheckOverlap(ranWidth, ranHeight))
                {
                    ranWidth = Random.Range(minRoomWidth, maxRoomWidth + 1);
                    ranHeight = Random.Range(minRoomHeight, maxRoomHeight + 1);

                    stuckCount++;

                    if (stuckCount > 30)
                    {
                        GameObject.Destroy(roomPoints[rooms.Count]);
                        roomPoints.RemoveAt(rooms.Count);
                        numberOfRooms = roomPoints.Count;
                        return;
                    }
                }
            }

            Vector3 roomPos = roomPoints[rooms.Count].transform.position + new Vector3(0, 0, 1);

            // If I destroy the room points it will generate new ones so it look soff, guess they'll just have to stay for now
            //GameObject.Destroy(roomPoints[0]);
            //roomPoints.RemoveAt(0);

            rooms.Add(Instantiate(roomFloor, roomPos, Quaternion.identity));
            rooms[rooms.Count - 1].transform.localScale = new Vector3(ranWidth, ranHeight, 0);
        }
    }

    private bool CheckOverlap(int width, int height)
    {
        float xMin = rooms[rooms.Count - 1].transform.position.x - rooms[rooms.Count - 1].transform.localScale.x / 2;
        float xMax = rooms[rooms.Count - 1].transform.position.x + rooms[rooms.Count - 1].transform.localScale.x / 2;
        float yMax = rooms[rooms.Count - 1].transform.position.y + rooms[rooms.Count - 1].transform.localScale.y / 2;
        float yMin = rooms[rooms.Count - 1].transform.position.y - rooms[rooms.Count - 1].transform.localScale.y / 2;

        for (int i = 0; i < rooms.Count - 1; i++)
        {
            float xMin_ = rooms[i].transform.position.x - rooms[i].transform.localScale.x / 2;
            float xMax_ = rooms[i].transform.position.x + rooms[i].transform.localScale.x / 2;
            float yMax_ = rooms[i].transform.position.y + rooms[i].transform.localScale.y / 2;
            float yMin_ = rooms[i].transform.position.y - rooms[i].transform.localScale.y / 2;

            if (xMin < xMax_ && xMax > xMin_ &&
                yMin < yMax_ && yMax > yMin_)
            {
                return true;
            }
        }
        return false;
    }
}
